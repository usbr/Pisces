# Two columns - Date and Value
# End of month
SELECT DateValue(Month(Working_Set.PeriodEnd) & "/" & Day(Working_Set.PeriodEnd) & "/" & IIf(Month(Working_Set.PeriodEnd)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)) AS mmddyyyy, Working_Set.Data
FROM Working_Set
WHERE (((Working_Set.PlantName)="BRNLEE") AND ((Working_Set.DataType)="ENDSTO") AND (Day(Working_Set.PeriodEnd)>27) 
AND (DateValue(Month([Working_Set].[PeriodEnd]) & "/" & Day([Working_Set].[PeriodEnd]) & "/" & IIf(Month([Working_Set].[PeriodEnd])>9,Left([Working_Set].[WySeq],4),Left([Working_Set].[WySeq],4)+1)))>=#10/15/1950# 
AND (DateValue(Month([Working_Set].[PeriodEnd]) & "/" & Day([Working_Set].[PeriodEnd]) & "/" & IIf(Month([Working_Set].[PeriodEnd])>9,Left([Working_Set].[WySeq],4),Left([Working_Set].[WySeq],4)+1)))<=#10/15/1960#);

# End of month using PeriodStart to calculate end of month
SELECT DateValue(Month(Working_Set.PeriodStart) & "/" & Day(DateAdd("d", -1, DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1), Month(Working_Set.PeriodStart)+1,1))) & "/" & IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)) AS mmddyyyy, Working_Set.Data
FROM Working_Set
WHERE (((Working_Set.PlantName)="ALBENI") AND ((Working_Set.DataType)="ENDSTO") AND (Day(Working_Set.PeriodEnd)>27)
AND DateValue(Month(Working_Set.PeriodStart) & "/" & Day(DateAdd("d", -1, DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1), Month(Working_Set.PeriodStart)+1,1))) & "/" & IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1))>=#10/15/1950#
AND DateValue(Month(Working_Set.PeriodStart) & "/" & Day(DateAdd("d", -1, DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1), Month(Working_Set.PeriodStart)+1,1))) & "/" & IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1))<=#10/15/1960#);

# Average April and August to the 15th of the month
SELECT DateValue(Month(Working_Set.PeriodStart) & "/15/" & IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)) AS mmddyyyy, Avg(Working_Set.Data) AS AvgOfData
FROM Working_Set
GROUP BY DateValue(Month(Working_Set.PeriodStart) & "/15/" & IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)), Working_Set.PlantName, Working_Set.DataType
HAVING (((DateValue(Month(Working_Set.PeriodStart) & "/15/" & IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)))>=#10/15/1950# And (DateValue(Month(Working_Set.PeriodStart) & "/15/" & IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)))<=#10/15/1960#) AND ((Working_Set.PlantName)="BRNLEE") AND ((Working_Set.DataType)="ENDSTO"));

# Average April and August to the end of month
SELECT DateValue(Month(Working_Set.PeriodStart) & "/" & Day(DateAdd("d", -1, DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1), Month(Working_Set.PeriodStart)+1,1))) & "/" & IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)) AS mmddyyyy, Avg(Working_Set.Data) AS AvgOfData
FROM Working_Set
GROUP BY DateValue(Month(Working_Set.PeriodStart) & "/" & Day(DateAdd("d", -1, DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1), Month(Working_Set.PeriodStart)+1,1))) & "/" & IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)), Working_Set.PlantName, Working_Set.DataType
HAVING (((Working_Set.PlantName)="BRNLEE") AND ((Working_Set.DataType)="QOUT") AND ((DateValue(Month([Working_Set].[PeriodStart]) & "/" & Day(DateAdd("d", -1, DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1), Month(Working_Set.PeriodStart)+1,1))) & "/" & IIf(Month([Working_Set].[PeriodStart])>9,Left([Working_Set].[WySeq],4),Left([Working_Set].[WySeq],4)+1)))>=#10/15/1950# And (DateValue(Month([Working_Set].[PeriodStart]) & "/" & Day(DateAdd("d", -1, DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1), Month(Working_Set.PeriodStart)+1,1))) & "/" & IIf(Month([Working_Set].[PeriodStart])>9,Left([Working_Set].[WySeq],4),Left([Working_Set].[WySeq],4)+1)))<=#10/15/1960#));