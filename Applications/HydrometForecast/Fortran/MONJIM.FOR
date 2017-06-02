      PROGRAM MONREP
C
      INTEGER CODE,THISYR
      INTEGER BEG,END,T
C
      DIMENSION RLCONT(54),RTMON(54),AVECONT(54),ACTOT(55)
      DIMENSION T2(25),T3(25),T4(25),T5(25),T6(25),T8(25),T7(25)
      DIMENSION BEG(23),END(23),STITLE(25,21),TITLE(55,21)
      DIMENSION TOT2(25),TOT5(25),TOT7(25),TOT3(25)
      DIMENSION TOT4(25),TOT6(25),TOT8(25) 
      DIMENSION TOT1(25),RLMON(54),IYRR(2)


      DIMENSION OUT(54),PRE(54),AVPRE(54),DATA(12)
C
      CHARACTER*4 YEAR
      CHARACTER*12 CBT(54),SCB(54),STCH(4),RESC(4)
      CHARACTER*25 ADD
      CHARACTER*9 IDATA(12),IBLANK,LCONT(54),LMON(54),TMON(54),TT(54)
      CHARACTER*9 IAVECONT(54),IPRE(54),IAVPRE(54),IOUT(54)
      CHARACTER*8 IOT2(25),IOT4(25),IOT5(25),IOT7(25),IOT8(25)
      CHARACTER*7 IOT1(25),IOT3(25)
      CHARACTER*9 IOT6(25),PCODE(5)
	PARAMETER NYEAR=1900
C
	INCLUDE 'DRA2:[MPOLL]OPEN.MP/LIST'
	OPEN(UNIT=2,CARRIAGECONTROL='LIST',STATUS='NEW')
	OPEN(UNIT=3,CARRIAGECONTROL='LIST',STATUS='UNKNOWN')
	DATA CBT/'AGA         ','AMF         ','AND         ','ARK         ',
	1        'BNK         ','BEU         ','BUL         ','BUM         ',
	2        'CSC         ','CLE         ','CLR         ','CLS         ',
	3        'CCR         ','CRA         ','CRE         ','DED         ',
	4        'EMI         ','GCL         ','FIS         ','FOR         ',
	5        'GRS         ','HAY         ','SCO         ','HPD         ',
	6        'HGH         ','HYA         ','ISL         ','JCK         ',
	7        'KAC         ','KEE         ','LOW         ','CMO         ',
	8        'MIN         ','WOD         ','LUC         ','MAN         ',
	9        'MCK         ','OCH         ','OWY         ','PAL         ',
	1        'PHL         ','POT         ','PRV         ','RES         ',
	2        'RIM         ','RIR         ','CCL         ','SOL         ',
	3        'THF         ','UNY         ','WAR         ','WAS         ',
	4        'WIC         ','WLD         '/
	DATA SCB/'PHL         ','THF         ','AND         ','ARK         ',
	1        'LUC         ','LOW         ','BNK         ','POT         ',
	2        'OCH         ','PRV         ','CRA         ','WIC         ',
	3        'HAY         ','CRE         ','SCO         ','CMO         ',
	4        'RES         ','SOL         ','WOD         ','MAN         ',
	5        'AMF         ','GRS         ','ISL         ','JCK         ',
	6        'PAL         ','RIR         ','MIN         ','CCL         ',
	7        'CCR         ','OWY         ','CSC         ','DED         ',
	8        'AGA         ','EMI         ','HPD         ','HYA         ',
	9        'FOR         ','FIS         ','CLS         ','MCK         ',
	1        'UNY         ','BEU         ','WAR         ','BUL         ',
	2        'WAS         ','WLD         ','BUM         ','CLE         ',
	3        'CLR         ','KAC         ','KEE         ','RIM         ',
	4        'GCL         ','HGH         '/
	DATA STCH/'PRIO        ','OCWO        ','UNY         ','JCKY        '/
	DATA RESC/'PRV         ','OCH         ','UNY         ','JCK         '/
      DATA BEG/1,3,7,9,11,15,16,17,19,20,21,28,30,31,33,39,41,42,
     1         45,46,47,53,54/
      DATA END/2,6,8,10,14,15,16,18,19,20,27,29,30,32,38,40,41,44,45,
     2         46,52,53,54/
      DATA PCODE/'AF       ','AFA      ','OM       ','PM       ',
	1        'PMA      '/
C
1000  FORMAT(A4,2I2,12A6)
C     
C     INITIALIZE ADDITION TOTALS FOR SUMARY REPORT
C
      IBLANK='         '
      DO 200 I=1,25
      TOT1(I)=0.
      TOT2(I)=0.
      TOT5(I)=0.
      TOT7(I)=0.
  200 CONTINUE
      CALL IDATE(MON,IDA,IYR)
	MON=9
	IYR=93
      MOL=MON
	MOL=MOL-1
   	NYR=IYR
      IF(MON.EQ.1)THEN
                  NYR=NYR-1
                  MOL=12
                  END IF
C
	MON=MON-1
C     NEXT DETERMINE LAST YEAR THIS YEAR MONTHLY DATA LOCATION ON
C     REGDATA FILE
      IF(MON.GT.9)THEN
                  LASTYR=IYR+NYEAR
                  THISYR=IYR+1+NYEAR
                  LOCATE=MON-9
      END IF
      IF(MON.LE.9)THEN
                  LASTYR=IYR-1+NYEAR
                  THISYR=IYR+NYEAR
                  LOCATE=MON+3
      END IF
C
      DO 403 K=1,55
      IF(K.GT.54)THEN
		READ(3,1003)(TITLE(K,I),I=1,21)
		GO TO 403
		END IF
      READ(3,1002)(TITLE(K,I),I=1,21),ACTOT(K)
      ACTOT(55)=ACTOT(55)+ACTOT(K)
  403 CONTINUE
 1002 FORMAT(21A1,F7.1)
      DO 404 K=1,25
      READ(3,1003)(STITLE(K,I),I=1,21)
 1003 FORMAT(21A1)
  404 CONTINUE
C     
	IYRR(2)=LASTYR
	IYRR(1)=THISYR
      DO 41 LINE=1,54
      KK=1
      DO 42 IK=1,5
   40 ENCODE(4,1023,YEAR)IYRR(KK)	
1023  FORMAT(I4)
       	IF(IK.NE.2.OR.IK.NE.5)ADD=CBT(LINE)//PCODE(IK)//YEAR
	IF(IK.EQ.2.OR.IK.EQ.5)ADD=CBT(LINE)//PCODE(IK)//'9999'
	IF(IK.EQ.4.OR.IK.EQ.5)THEN
		DO 47 IR=1,4
		IF(CBT(LINE).EQ.RESC(IR))ADD(1:12)=STCH(IR)
47		CONTINUE
		END IF
 501  READ(UNIT=1,KEY=ADD,KEYID=0,IOSTAT=IOS,ERR=9998)ADD,DATA
1028	FORMAT(1X,'ADD=',A,/,1X,6A,/,1X,6A)
	UNLOCK 1
	GO TO 502
C
C                 ******ERROR ON READ******
C------------------------------------------------------------------
C
9998	IF(IOS.EQ.52)CALL RECLOCK(*501,*5001)
	DO 44 IZ=1,12
	DATA(IZ)=998877.0
44	CONTINUE
	WRITE(6,1022)IOS,ADD
1022	FORMAT(1X,'ERROR ON READ IOS=',I2,3X,'ADD=',A)
C
C------------------------------------------------------------------
C 
502	DO 43 I=1,12
 	ENCODE(9,1020,IDATA(I),ERR=9997)DATA(I)
	GO TO 503
9997	WRITE(6,1024)ADD,(DATA(J),J=1,12)
1024	FORMAT(1X,'ERROR ON ENCODE 1020',A,/,1X,6F12.2,/,1X,6F12.2)
	IDATA(I)='         '
	DATA(I)=0.
	GO TO 42
1020	FORMAT(F9.2)
503        IF(DATA(I).EQ.998877.0)THEN
		IDATA(I)='         '
		DATA(I)=0.
		END IF
43	CONTINUE 
!	WRITE(6,1028)ADD,(IDATA(IA),IA=1,12)
	IF(DATA(LOCATE).GE.9999.)WRITE(6,1029)ADD,DATA(LOCATE)
1029	FORMAT(1X,'CHECK DATA FOR ADD=',A,F9.2)
C
C      LOCATING THIS YEAR AND LAST YEAR EOM CONTENT
      IF(PCODE(IK).EQ.'AF       ')THEN
	      IF(IYRR(KK).EQ.LASTYR) THEN
                 RLCONT(LINE)=DATA(LOCATE)
                 IF(MON.EQ.10)LMON(LINE)=IDATA(LOCATE+11)
	         GO TO 400
	      END IF
 	     IF(IYRR(KK).EQ.THISYR) THEN
                 LL=LOCATE-1
                 IF(MON.EQ.10)THEN
			RLMON(LINE)=DATA(LOCATE)
			GO TO 2000
			END IF
                 LMON(LINE)=IDATA(LL)
                 RLMON(LINE)=DATA(LL)
2000             TMON(LINE)=IDATA(LOCATE)
                 RTMON(LINE)=DATA(LOCATE)
	      END IF
	      GO TO 400	
C
C     LOCATING THE 1963-1977 AVERAGE EOM CONTENT
      ELSE IF(PCODE(IK).EQ.'AFA      ')THEN
      	IAVECONT(LINE)=IDATA(LOCATE)
        AVECONT(LINE)=DATA(LOCATE)
        GO TO 400
C
C     LOCATING THE OUTFLOW DATA ON REGDATA FILE
      ELSE IF(PCODE(IK).EQ.'OM       ')THEN
     	 IOUT(LINE)=IDATA(LOCATE)
      	 OUT(LINE)=DATA(LOCATE)
         GO TO 42
C
C     LOCATING THE PRECIPITATION DATA ON REGDATA FILE
      ELSE IF(PCODE(IK).EQ.'PM       ')THEN
         IPRE(LINE)=IDATA(LOCATE)
         PRE(LINE)=DATA(LOCATE)
         GO TO 42
C
C     LOCATING AVERAGE PRECIPITATION DATA
      ELSE IF(PCODE(IK).EQ.'PMA      ')THEN
         IAVPRE(LINE)=IDATA(LOCATE)
         AVPRE(LINE)=DATA(LOCATE)
         GO TO 42
      END IF
C
C
C      GROUPING LOGIC-- GROUPS RESERVOIRS INTO PROJECTS FOR SUMARY REPORT
  400 DO 401 IH=1,54
      IF(CBT(LINE).EQ.SCB(IH)) GO TO 402
  401 CONTINUE
      GO TO 42
C
C     LOCATING WHICH PROJECT THE RESERVOIR BELONGS
  402 IF(IH.GT.0.AND.IH.LT.3)K=1
      IF(IH.GT.2.AND.IH.LT.7)K=2
      IF(IH.GT.6.AND.IH.LT.9)K=3
      IF(IH.GT.8.AND.IH.LT.11)K=4
      IF(IH.GT.10.AND.IH.LT.15)K=5
      IF(IH.EQ.15)K=6
      IF(IH.EQ.16)K=7
      IF(IH.GT.16.AND.IH.LT.19)K=8
      IF(IH.EQ.19)K=9
      IF(IH.EQ.20)K=10
      IF(IH.GT.20.AND.IH.LT.28)K=11
      IF(IH.GT.27.AND.IH.LT.30)K=12
      IF(IH.EQ.30)K=13
      IF(IH.GT.30.AND.IH.LT.33)K=14
      IF(IH.GT.32.AND.IH.LT.39)K=15
      IF(IH.GT.38.AND.IH.LT.41)K=16
      IF(IH.EQ.41)K=17
      IF(IH.GT.41.AND.IH.LT.45)K=18
      IF(IH.EQ.45)K=19
      IF(IH.EQ.46)K=20
      IF(IH.GT.46.AND.IH.LT.53)K=21
      IF(IH.EQ.53)K=23
      IF(IH.EQ.54)K=24
C 
C     GROUPING DATA CALCULATIONS
    1 IF(PCODE(IK).EQ.'AFA      ')THEN
                   TOT7(K)=TOT7(K)+AVECONT(LINE)
		   TOT1(K)=TOT1(K)+ACTOT(LINE)
      END IF
      IF(PCODE(IK).EQ.'AF       ')THEN
                   IF(IYRR(KK).EQ.LASTYR)THEN
                     TOT5(K)=TOT5(K) +RLCONT(LINE)
			KK=1
			GO TO 418
                     END IF
                   IF(IYRR(KK).EQ.THISYR)THEN
                     TOT2(K)=TOT2(K)+RTMON(LINE)
                     TT(IH)=TMON(LINE)
			KK=2
                     END IF
      END IF
418	IF(PCODE(IK).EQ.'AF       '.AND.KK.EQ.2)GO TO 40
42	CONTINUE
41	CONTINUE
C
C     CALCULATING TOTAL RESERVOIR CONTENT,OUTFLOW,AVERAGE PRECIP,
C                 PRECIPITATION FOR MONTHLY REPORT
  100 DO 102 I=1,54
      IF(LMON(I).NE.IBLANK.AND.TMON(I).NE.IBLANK)THEN
                            TRLMON=TRLMON+RLMON(I)
                            TRTMON=TRTMON+RTMON(I)
                            TAVECO=TAVECO+AVECONT(I)
                            TOUT=TOUT+OUT(I)
      END IF
      IF(IPRE(I).NE.IBLANK.AND.IAVPRE(I).NE.IBLANK)THEN
                            TAVPRE=TAVPRE+AVPRE(I)
                            TPRE=TPRE+PRE(I)
      END IF
  102 CONTINUE
      GO TO 500
C
C      LOADING RESERVOIR AND PROJECT NAME DATA 

500   TOT1(22)=0.
      DO 405 I=1,21
      TOT1(22)=TOT1(22)+TOT1(I)
      TOT7(22)=TOT7(22)+TOT7(I)
      TOT5(22)=TOT5(22)+TOT5(I)
      TOT2(22)=TOT2(22)+TOT2(I)
  405 CONTINUE
C
C
      LF=0
      DO 412 K=1,21
      DO 411 I=BEG(K),END(K)
      IF(TT(I).EQ.IBLANK)LF=LF+1
  411 CONTINUE
      KC=END(K)-BEG(K)+1
      IF(LF.EQ.KC)THEN
                  TOT7(22)=TOT7(22)-TOT7(K)
                  TOT1(22)=TOT1(22)-TOT1(K)
                  T2(K)=-999999.
                  T4(K)=-999999.
                  T5(K)=-999999.
                  T6(K)=-999999.
		  T7(K)=-999999.
                  T8(K)=-999999.
                  T3(K)=-999999.
      END IF
      LF=0
  412 CONTINUE
      TOT1(25)=0.
      DO 406 I=22,24
      TOT1(25)=TOT1(25)+TOT1(I)
      TOT7(25)=TOT7(25)+TOT7(I)
      TOT5(25)=TOT5(25)+TOT5(I)
      TOT2(25)=TOT2(25)+TOT2(I)
  406 CONTINUE
C
      DO 407 I=1,25
      IF(TOT1(I).EQ.0)THEN
                      TOT3(I)=0.
                      GO TO 417
      END IF
      TOT3(I)=TOT2(I)/TOT1(I)*100
  417 TOT4(I)=TOT1(I)-TOT2(I)
      TOT6(I)=TOT2(I)-TOT5(I)
      TOT8(I)=TOT2(I)-TOT7(I)
      ENCODE(7,1010,IOT1(I))TOT1(I)
      ENCODE(8,1011,IOT2(I))TOT2(I)
      ENCODE(7,1013,IOT3(I))TOT3(I)
      ENCODE(8,1011,IOT4(I))TOT4(I)
      ENCODE(8,1011,IOT5(I))TOT5(I)
      ENCODE(9,1012,IOT6(I))TOT6(I)
      ENCODE(8,1011,IOT7(I))TOT7(I)
      ENCODE(8,1011,IOT8(I))TOT8(I)
      IF(T2(I).EQ.-999999.)IOT2(I)='        '
      IF(T3(I).EQ.-999999.)IOT3(I)='       '
      IF(T4(I).EQ.-999999.)IOT4(I)='        '
      IF(T5(I).EQ.-999999.)IOT5(I)='        '
      IF(T6(I).EQ.-999999.)IOT6(I)='         '
      IF(T8(I).EQ.-999999.)IOT8(I)='        '
      IF(T7(I).EQ.-999999.)IOT7(I)='        '
  407 CONTINUE
 1010 FORMAT(F7.1)
 1011 FORMAT(F8.1)
 1012 FORMAT(F9.1)
 1013 FORMAT(F7.0)
C
C      PRINT OUT MONTHLY REPORT
C**********************************************************************
C
      T=NYR
      M=MOL
      WRITE(2,1004)T,M,T,M,T,M,T,M,T,M
 1004 FORMAT(1H1,35X,'BUREAU OF RECLAMATION--PN REGION',/,38
     +X,'OUTLOOK FOR WATER-REPORT FOR   ',I2,'/',I2,/,47X,
     +'(1000 ACRE-FEET)',/,10X,89(1H-),/,10X,'   RESERVOIR           ACT  
     +IVE    BEGINING    END     AVERAGE   OUTFLOW   PRECIPITATION',
     +/10X,'  OR STATION          CAPACITY   ',I2,'/',I2,5X,I2,'/',
     +I2,5X,'E.O.M.',4X,I2,'/',I2,5X,I2,'/',I2,8X,'AVG'/10X,89(1H-))
      DO 408 K=1,54
      WRITE(2,1005)(TITLE(K,I),I=1,21),ACTOT(K),LMON(K),TMON(K),
     +IAVECONT(K),IOUT(K),IPRE(K),IAVPRE(K)
  408 CONTINUE
 1005 FORMAT(10X,21A1,F7.1,2X,A9,5(1X,A9))
      WRITE(2,1006)(TITLE(55,I),I=1,21),ACTOT(K),TRLMON,TRTMON,TAVECO,
     +             TOUT,TPRE,TAVPRE
 1006 FORMAT(10X,89(1H-)/10X,21A1,F7.1,F11.1,3F10.1,2F10.2)
C
C     PRINTOUT SUMMARY REPORT
	DO 1117 I=1,5
	WRITE(2,1118)
1118	FORMAT(' ')
1117	CONTINUE
      WRITE(2,1007)NYR,MOL
 1007 FORMAT(1H1,35X,'BUREAU OF RECLAMATION',/,
     +       30X,'SUMMARY OF E.O.M. ACTIVE STORAGE CONTENTS',/,
     +       43X,'(1000 ACRE--FEET)',/,48X,I2,'/',I2,/,10X,86(1H-)/,
     +      13X,'RESERVOIR          ACTIVE   E.O.M. PERCENT  SPACE    LA
     +ST    CHANGE AVERAGE  CHANGE',/,16X,'OR',13X,'CAPACITY CONTENTS FU
     +LL  AVAILABLE  YEAR     FROM  CONTENTS  FROM',/,14X,
     +'PROJECT',43X,'CONTENTS   LAST',10X,'AVERAGE',/,75X,'YEAR',/,
     +       10X,86(1H-))
      DO 409 K=1,21
      WRITE(2,1008)(STITLE(K,I),I=1,21),IOT1(K),IOT2(K),IOT3(K),
     +             IOT4(K),IOT5(K),IOT6(K),IOT7(K),IOT8(K)
  409 CONTINUE
 1008 FORMAT(10X,21A1,A7,A8,A7,2A8,A9,2A8)
      WRITE(2,1009)
 1009 FORMAT(10X,86(1H-))
      DO 410 K=22,24
      WRITE(2,1008)(STITLE(K,I),I=1,21),IOT1(K),IOT2(K),IOT3(K),
     +             IOT4(K),IOT5(K),IOT6(K),IOT7(K),IOT8(K)
  410 CONTINUE
      WRITE(2,1009)
      WRITE(2,1008)(STITLE(25,I),I=1,21),IOT1(25),IOT2(25),IOT3(25),
     +             IOT4(25),IOT5(25),IOT6(25),IOT7(25),IOT8(25)
      WRITE(2,1019)
1019  FORMAT(/////,10X,'_1/  One half of Warm Springs Reservoir ',
     +'(95,500 AF) belongs to the Vale Project.',/,15X,'The other',
     +' half belongs to Warm Springs Irrigation District.',/,15X,
     +'The total storage available to the Vale Project is 185,500 AF.')
      GO TO 5001
9999  WRITE(6,1021)IOS
1021  FORMAT(1X,'ERROR ON OPEN -- IOS=',I2)
5001  STOP
      END
