#!/bin/bash
# linux Pisces test script
mono ./nuget.exe  install Nunit.Runners
mono  ./NUnit.ConsoleRunner.3.4.1/tools/nunit3-console.exe ./TimeSeries/bin/Debug/Reclamation.TimeSeries.dll /exclude:DatabaseServer /exclude:Internal


