using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.Reflection;

namespace Reclamation.TimeSeries.Parser
{

    /// <summary>
    /// Manage Function calls and utility methods for Function metadata
    /// </summary>
    public class ParserFunction
    {
        public string Name { get; set; }
        public ParserFunction()
        {
            Parameters = new string[] { };
            Name = "";
        }

        public string[] Parameters { get; set; }



        public static FunctionAttribute[] GetPiscesFunctionAttributes()
        {
            var rval = new SortedList<string, FunctionAttribute>();
            Type t = typeof(Reclamation.TimeSeries.Math);

            //var tList = Assembly.GetExecutingAssembly().GetTypes();
            //for (int i = 0; i < length; i++)
            //{
                
            //}

            MemberInfo info = t;

            var methods = t.GetMethods();
            // crete some markdown
            Console.WriteLine("# List of Pisces Functions");
            Console.WriteLine("Name |Description|Example usage");
            Console.WriteLine("----| ----|-----");

            for (int i = 0; i < methods.Length; i++)
            {
                var attributes = methods[i].GetCustomAttributes(typeof(FunctionAttribute), false);
                if (attributes.Length > 0)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j] is FunctionAttribute)
                        {
                            var a = attributes[j] as FunctionAttribute;
                            string key = methods[i].Name;
                            if (rval.IndexOfKey(key) >= 0)
                            {
                                key = key + j;
                            }
                            Console.WriteLine(key+"|"+a.Description+"|Example: " +a.Example);
                            rval.Add(key,new FunctionAttribute(a.Description,a.Example,methods[i].Name));
                            break;
                        }
                    }


                }
            }

            var x = (from fa in rval select fa.Value).ToArray<FunctionAttribute>();
            return x;
        }


        public ParserResult Evaluate(List<ParserResult> args)
        {
            try
            {

                Type t = typeof(Reclamation.TimeSeries.Math);

                List<Type> types = new List<Type>();
                List<object> parameters = new List<object>();
                for (int a = 0; a < args.Count; a++)
                {
                    if (args[a].IsSeries)
                    {
                        types.Add(typeof(Series));
                        parameters.Add(args[a].Series);
                    }
                    else if (args[a].IsDouble)
                    {
                        types.Add(typeof(double));
                        parameters.Add(args[a].Double);
                    }
                    else if (args[a].IsInteger)
                    {
                        types.Add(typeof(int));
                        parameters.Add(args[a].Integer);
                    }
                    else if (args[a].IsBool)
                    {
                        types.Add(typeof(bool));
                        parameters.Add(args[a].Bool);
                    }
                    else if (args[a].IsString)
                    {
                        types.Add(typeof(string));
                        parameters.Add(args[a].Str);
                    }
                }

                var methodInfo = t.GetMethod(Name, types.ToArray());
                object o = null;
                if (methodInfo == null)
                {// may need to combined types into single array 

                    methodInfo = t.GetMethod(Name);
                    if (methodInfo == null)
                    {
                        throw new MissingMethodException("Could not find method '"+Name+"'");
                    }

                    if (IsParamsMethod(methodInfo))
                    {
                        o = InvokeParamsMethod(null, methodInfo, parameters.ToArray());
                    }
                    else
                    {
                      // add default parameters for any not passed.
                        var mParms = methodInfo.GetParameters();

                        for (int i = parameters.Count; i < mParms.Length; i++)
                        {
                            parameters.Add(mParms[i].DefaultValue);
                        }
                        o = methodInfo.Invoke(null,parameters.ToArray());
                    }


                }
                else
                { 
                    o = methodInfo.Invoke(null, parameters.ToArray());
                }
                if (o is Series)
                    return new ParserResult((o as Series));
                else if (o is double)
                    return new ParserResult((double)o);

                else throw new InvalidOperationException(Name);
            }
            catch (Exception exc)
            {
                string msg = exc.Message;
                if(exc.InnerException != null)
                    msg+="\n" + exc.InnerException.Message+ " "+exc.InnerException.StackTrace;
                Logger.WriteLine(msg);
                throw new ParserException(msg);
            }
        }




        public static bool IsParamsMethod(System.Reflection.MethodInfo mi)
        {
            System.Reflection.ParameterInfo[] parameters = mi.GetParameters();

            if (parameters.Length > 0)
                return System.Attribute.IsDefined(parameters[parameters.Length - 1], typeof(ParamArrayAttribute));
            else
                return false;
        }



        /// <summary>
        /// from http://www.codeproject.com/KB/IP/DynamicGeneration.aspx
        /// </summary>
        /// <param name="callee"></param>
        /// <param name="methodInfo"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static object InvokeParamsMethod(object callee,
    System.Reflection.MethodInfo methodInfo, object[] parameters)
        {
            /*
             * all parameters, except the last remain the same
             * the last parameter is an array holding all the 
             *parameters that come as the 'params' argument
             */
            System.Reflection.ParameterInfo[] pInfos = methodInfo.GetParameters();
            object[] finalParameters = new object[pInfos.Length];

            for (int index = 0; index < pInfos.Length - 1; ++index)
                finalParameters[index] = parameters[index];
            Array paramsArray =
                Array.CreateInstance(
                Type.GetType(pInfos[pInfos.Length - 1].ParameterType.ToString(
                ).Replace("[]", "")),
                parameters.Length - (pInfos.Length - 1)
                );
            System.Array.Copy(
                parameters,
                pInfos.Length - 1,
                paramsArray,
                0,
                paramsArray.Length
            );
            finalParameters[pInfos.Length - 1] = paramsArray;

            return methodInfo.Invoke(callee, finalParameters);
        }
             
    }
}
