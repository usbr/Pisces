yakhyd$ty statout.for
C
C       PROGRAM STATOUT.FOR
C
C       PROGRAM TO OUTPUT DAILY STATUS REPORT
C       Any time of day on the hour WITH FOREBAY ELEVATION INCLUDED.
C       Forebay elevations can be shown on output left off.
C
c************************************************************
c*    Modification:
c*      April, 1998   Bob Modrell, Vitel, Inc.
c*        Move to Digital Equipment Corporation (DEC)
c*        OpenVMS (TM) Alpha Operating System Version V7.1
c*        and DEC Fortran 77.
c*      May 7, 1998 bam
c*        Changes made to handle problem of Y2K
c*      12/06/2000 mp
c*        update to add new stations, match format to statout
c*      October 30, 2001 bam
c*        Modifications made to replace the station Yakima
c*        River near Ellensburg (ELNW) with the station
c*        Yakima River near Horlick (YRWW).  Added the logic
c*        needed for users to run a report for any hour of
c*        the day and for users to specify if they want
c*        FOREBAY ELEVATIONS to show on output.
c*      May 19, 2005 JDoty
c*        Changed Bumping capacity from 33,700 to 33,970 AF.
c*      Nov 2015 Ktarbet
c*         change TICW abreviation and add CLR, drop YUMW
c************************************************************
c*    Compile and link programs with the following commands:
c*        fortran/extend/align/warn=nousage statout
c*        link/exe=huser1:[lib.prod]statout.exe statout
c************************************************************
c
C       this program called by status.com
C           depends on output of status.exe
C       FOR001 - INPUT FILE, STATUS.DAT with dayfiles output
C       FOR002 - OUTPUT FILE, STATUS.OUT
C
        DIMENSION NDAYS(12),AF(20,2),Q(20,2),CAPS(6),FB(20,2)
        DIMENSION IYR(2),IMON(2),IDAY(2),QC(20,2),PCNT(6)
C
        REAL NFLO(5)
C
        DATA NDAYS /31,28,31,30,31,30,31,31,30,31,30,31/
C
        CHARACTER*8 TIM
        CHARACTER*9 CDATE
        CHARACTER*4 CBT1
        CHARACTER*3 PCODE,TMON,CMON(12)
        CHARACTER*80 COMMENTS(15),BLANK
        CHARACTER*50 STRING
        character*2 ctim                !11/07/2001
        CHARACTER*1 QUES, fbelev        !11/07/2001
        CHARACTER*4 CHEK
        character*24 timbuf             !5/7/98 bam
        integer*4 timlen                !5/7/98 bam
        integer*2 settime, itim         !11/07/2001
C
C------------------------------------------------------------------------
C
        DATA CMON/'JAN','FEB','MAR','APR','MAY','JUN','JUL',
        1'AUG','SEP','OCT','NOV','DEC'/
        DATA CAPS/157800.,239000.,436900.,33970.,198000.,
        11065400./
C
        IN1=1   !  INPUT DATA FILE containing dayfile output
        IO2=2   !  REPORT OUTPUT FILE, status.out
        IN5=5   !  Keyboard INPUT by user for small users and
                !  operational comments
        in7 = 7 !  10/30/2001
                !  Input from user in program STATUS.FOR and
                !  contains time of run and Y for yes and N for no
                !  if Forebay Elevations are required on output.
C
        INOVAL=0
        NOVAL=0.0
        BLANK=' '
        timlen = 23             !5/7/98 bam
C
C       ZERO OUT ARRAY
C
        DO 55 M=1,20
        DO 50 N=1,2
        AF(M,N)=0.0
        Q(M,N)=0.0
        QC(M,N)=0.0
        FB(M,N)=0.0             !12/6/00 mp
50      CONTINUE
55      CONTINUE
C
        DO 60 I=1,15
        COMMENTS(I)=' '
60      CONTINUE
C
C
c       CALL IDATE(IMON(2),IDAY(2),IYR(2))
c     The following 8 statements were added 5/7/98 bam
c    Get VAX time in the following form: 05-MAY-1998 10:30:45.21
        call sys$asctim(timlen, timbuf)
        read(timbuf,10) iday(2), iyr(2), ihr, imin, isec
10      format(i2, 5x, i4, 3(1x, i2))
        do 12 i = 1,12
           if(timbuf(4:6) .eq. cmon(i)) go to 14
12      continue
14      imon(2) = i
 
C
        LPYR=IYR(2)/4
        IF((LPYR*4).EQ.IYR(2)) NDAYS(2)=29
C
C       COMPUTE YESTERDAY
C
        IMON(1)=IMON(2)
        IDAY(1)=IDAY(2)-1
        IF(IDAY(1).LE.0) THEN
                IMON(1)=IMON(2)-1
                IF(IMON(1).EQ.0) IMON(1)=12
                IDAY(1)=NDAYS(IMON(1))
        END IF
C
c       CALL TIME(TIM)
C
c       DECODE (8,500,TIM) IHR,IMIN,ISEC
c500    FORMAT(I2,1X,I2,1X,I2)
C
c       CALL DATE(CDATE)
C
c       Read input from temporary file - time of run and switch to tell
c       if Forebay Elevation is to be printed.  11/07/2001
        read(in7,20) settime
20      format(i2)
        if(settime .ne. 24) then
          write(ctim, '(i2)') settime
          itim = settime
        else
          ctim = '00'
          itim = 0
        end if
        do 22 k=1,2
          if(ctim(k:k) .eq. ' ') then
            ctim(k:k) = '0'
          end if
22      continue
        read(in7,25) fbelev
25      format(a1)
c
C       READ IN FROM KEYBOARD small diversions and COMMENTS FOR
C       PLANNED OPERATIONS.  TYPE KEYBOARD PROMPTS
C
        TYPE *,(' INPUT SMALL GUYS DIVERSION:')
        READ(IN5,81) DOU
81      FORMAT(F12.0)
C
        TYPE *,(' INPUT STUFF FOR SHORT RANGE PLANS:')
        DO 92 I=1,15
        READ(IN5,91) COMMENTS(I)
91      FORMAT(A80)
        IF(COMMENTS(I).EQ.BLANK) GO TO 93
92      CONTINUE
C
93      CONTINUE
C
C
C       READ INPUT DATA FROM STATUS.DAT FOR YESTERDAY AND TODAY
C----------------------------------------------------------------------
C
C               AF(1)=KEE STORAGE,AF(2)=KAC STORAGE,AF(3)=CLE STORAGE
C               AF(4)=BUM STORAGE,AF(5)=RIM STORAGE
C
C               Q(1)=KEE,Q(2)=KAC,Q(3)=CLE,Q(4)=BUM,Q(5)=RIM
C               Q(6)=EASW,Q(7)=YUMW,Q(8)=UMTW,Q(9)=NACW,Q(10)=PARW
C               Q(11)=YRPW,Q(12)=TNAW,Q(13)=YRWW,Q(14)=CLFW,Q(15)=TICW
C               Q(16)=RBDW
C
C               QC(1)=KTCW,QC(2)=WOPW,QC(3)=NSCW,QC(4)=SNCW,QC(5)=RSCW
C               QC(6)=TIEW,QC(7)=ROZW,QC(8)=WESW,QC(9)=KNCW,QC(10)=RZCW
C               QC(11)=CHCW
C
C
250     READ(IN1,777,END=300)STRING
777     FORMAT(A50)
        IF(STRING(1:4).EQ.'$DAY')GO TO 250                    !4/2/98 bam
        IF(STRING(1:4).EQ.'%W-D')GO TO 250                    !4/2/98 bam
        READ(STRING,730,END=300)CBT1,TMON,ITDAY,ITHR,ITMIN,PCODE,QUES,TDATA
730     FORMAT(5X,A4,5X,A3,3(1X,I2),3X,A3,5X,A1,1x,F10.2)     !4/2/98 bam
C
C
        DO 400 I=1,2
        IF((IDAY(I).EQ.ITDAY).AND.(CMON(IMON(I)).EQ.TMON)) THEN
                NRHR=ITHR+(ITMIN/60.+0.5)       !NRHR IS THE NEAREST WHOLE HR.
        IF(QUES.NE.BLANK)TDATA=0.0
          IF(NRHR.EQ.itim) THEN                 !11/07/2001
C           The following six statements added 09/17/1999  bam
            IF(PCODE.EQ.'FB ') THEN
              if(fbelev .eq. 'y' .or. fbelev .eq. 'Y') then       !11/07/2001
                IF(CBT1.EQ.'KEE ') fb(1,I)=TDATA
                IF(CBT1.EQ.'KAC ') fb(2,I)=TDATA
                IF(CBT1.EQ.'CLE ') fb(3,I)=TDATA
                IF(CBT1.EQ.'BUM ') fb(4,I)=TDATA
                IF(CBT1.EQ.'RIM ') fb(5,I)=TDATA
                IF(CBT1.EQ.'CLR') fb(6,I) =TDATA
              end if                                    !11/07/2001
            else IF(PCODE.EQ.'AF ') THEN            !09/17/1999 bam
              IF(CBT1.EQ.'KEE ') AF(1,I)=TDATA
              IF(CBT1.EQ.'KAC ') AF(2,I)=TDATA
              IF(CBT1.EQ.'CLE ') AF(3,I)=TDATA
              IF(CBT1.EQ.'BUM ') AF(4,I)=TDATA
              IF(CBT1.EQ.'RIM ') AF(5,I)=TDATA
            ELSE IF(PCODE.EQ.'Q  ') THEN
              IF(CBT1.EQ.'KEE ') Q(1,I)=TDATA
              IF(CBT1.EQ.'KAC ') Q(2,I)=TDATA
              IF(CBT1.EQ.'CLE ') Q(3,I)=TDATA
              IF(CBT1.EQ.'BUM ') Q(4,I)=TDATA
              IF(CBT1.EQ.'RIM ') Q(5,I)=TDATA
              IF(CBT1.EQ.'EASW') Q(6,I)=TDATA
              IF(CBT1.EQ.'YUMW') Q(7,I)=TDATA
              IF(CBT1.EQ.'UMTW') Q(8,I)=TDATA
              IF(CBT1.EQ.'NACW') Q(9,I)=TDATA
              IF(CBT1.EQ.'PARW') Q(10,I)=TDATA
              IF(CBT1.EQ.'YRPW') Q(11,I)=TDATA
              IF(CBT1.EQ.'TNAW') Q(12,I)=TDATA
              IF(CBT1.EQ.'YRWW') Q(13,I)=TDATA
              IF(CBT1.EQ.'CLFW') Q(14,I)=TDATA
              IF(CBT1.EQ.'TICW') Q(15,I)=TDATA
              IF(CBT1.EQ.'RBDW') Q(16,I)=TDATA
            ELSE IF(PCODE(1:2).EQ.'QC') THEN
              IF(CBT1.EQ.'KTCW') QC(1,I)=TDATA
              IF(CBT1.EQ.'WOPW') QC(2,I)=TDATA
              IF(CBT1.EQ.'NSCW') QC(3,I)=TDATA
              IF(CBT1.EQ.'SNCW') QC(4,I)=TDATA
              IF(CBT1.EQ.'RSCW') QC(5,I)=TDATA
              IF(CBT1.EQ.'TIEW') QC(6,I)=TDATA
              IF(CBT1.EQ.'ROZW') QC(7,I)=TDATA
              IF(CBT1.EQ.'WESW') QC(8,I)=TDATA
              IF(CBT1.EQ.'KNCW') QC(9,I)=TDATA
              IF(CBT1.EQ.'RZCW') QC(10,I)=TDATA
              IF(CBT1.EQ.'CHCW') QC(11,I)=TDATA
            END IF
          END IF
        END IF
400     CONTINUE
C
        GO TO 250
C
C       COMPUTE INFLOW TO FIVE RESERVOIRS
C
300     CONTINUE
        DO 350 I=1,5
        NFLO(I)=NOVAL
C-----  IF((Q(I,2).EQ.NOVAL).OR.(AF(I,2).EQ.NOVAL)
C-----  1.OR.(AF(I,1).EQ.NOVAL)) GO TO 350
        NFLO(I)=Q(I,2)+(AF(I,2)-AF(I,1))/1.9835
350     CONTINUE
C
        DO 360 I=1,5
        PCNT(I)=NOVAL
        IF(AF(I,2).EQ.NOVAL) GO TO 360
        PCNT(I)=(AF(I,2)/CAPS(I))*100.
360     CONTINUE
C
C       COMPUTE TOTAL STORAGE FOR ALL FIVE
C       BASIN RESERVOIRS
C---------------------------------------------------------------------
C
110     TREL=0.0
        TINFLO=0.0
        TSTOR=0.0
C
        DO 120 I=1,5
        IF(AF(I,2).EQ.NOVAL) THEN
                TSTOR=0.0
                GO TO 125
        END IF
        TSTOR=TSTOR+AF(I,2)
120     CONTINUE
C
125     DO 130 I=1,5
        IF(NFLO(I).EQ.NOVAL) THEN
                TINFLO=0.0
                GO TO 135
        END IF
        TINFLO=TINFLO+NFLO(I)
130     CONTINUE
C
135     DO 140 I=1,5
C-----  IF(Q(I,2).EQ.NOVAL) THEN
C-----          TREL=0.0
C-----          GO TO 145
C-----  END IF
        TREL=TREL+Q(I,2)
140     CONTINUE
C
C               COMPUTE % OF RESERVOIR FILL.
C-------------------------------------------------------------------------
C
145     PCNT(6)=NOVAL
        IF(TSTOR.NE.NOVAL.OR.TSTOR.NE.0.0)
        1 PCNT(6)=(TSTOR/CAPS(6))*100.
C
C               COMPUTE TOTAL DIVERSIONS.
C------------------------------------------------------------------------
C
        TDIV=0.
        DO 170 I=1,11
          IF(I.EQ.2.OR.I.EQ.3.OR.I.EQ.8.OR.I.EQ.9
     1    .OR.I.EQ.10.OR.I.EQ.11)GO TO 170
          TDIV=TDIV+QC(I,2)
170     CONTINUE
C
C               COMPUTE TRIBUTARY FLOW BELOW PARKER.
C------------------------------------------------------------------------
        TRIFLO=0.
        RESFLO=0.
        DO 160 I=1,5
        RESFLO=Q(I,2)+RESFLO
160     CONTINUE
C
C               TRIBUTARY FLOWS EQUAL DIVERSIONS #1+4+5
C               +6+7+DOU (see also comments in statout.for)
C-----------------------------------------------------------------------------
C
                DO 150 I=1,7
        IF(I.EQ.2.OR.I.EQ.3)GO TO 150
        TRIFLO=TRIFLO+QC(I,2)
150     CONTINUE
        TRIFLO=(TRIFLO+DOU+Q(10,2))-RESFLO
c
c       Test if Forebay Elevations are to be on output
C        if(fbelev .eq. 'y' .or. fbelev .eq. 'Y') go to 200
C  **************************************************************************
C               OUTPUT STATUS REPORT with Forebay Elevations  11/08/2001
C-----------------------------------------------------------------------------
C
200     WRITE(IO2,1510) timbuf(1:11), timbuf(13:20), ctim
1510    FORMAT(///,6X,A11,2X,A8,4X,'US BUREAU OF RECLAMATION',  !5/7/98 bam
     1  /,36X,'YAKIMA PROJECT',/,32X,'SYSTEM STATUS AT ', a2, ':00',
     2  //,18X,'FOREBAY',
     3  16X,'TOTAL',5X,'PERCENT',3X,'RESERVOIR',1X,'RESERVOIR',/,
     4  6X,'RESERVOIR',2x,'ELEVATION',3X,'CONTENT',4X,'CAPACITY',3X,'CAPACITY',
     5  5X,'INFLOW',2X,'RELEASES',/6X,74('-'),/,23X,'FB',9X,'AF',10X,
     6  'AF',9X,'%',9X,'CFS',7X,'CFS')
C  *****************************************************************************
*
C               OUTPUT RESERVOIR INFORMATION including Forebay Elevations.
C------------------------------------------------------------------------------
C  c    Added forebay to output next five statements.    09/17/1999  bam
        WRITE(IO2,1520) fb(1,2), AF(1,2),CAPS(1),PCNT(1),NFLO(1),Q(1,2)
        WRITE(IO2,1530) fb(2,2), AF(2,2),CAPS(2),PCNT(2),NFLO(2),Q(2,2)
        WRITE(IO2,1540) fb(3,2), AF(3,2),CAPS(3),PCNT(3),NFLO(3),Q(3,2)
        WRITE(IO2,1550) fb(4,2), AF(4,2),CAPS(4),PCNT(4),NFLO(4),Q(4,2)
        WRITE(IO2,1560) fb(5,2), AF(5,2),CAPS(5),PCNT(5),NFLO(5),Q(5,2)
        Write(IO2,1565) FB(6,2)
        WRITE(IO2,1570)TSTOR,CAPS(6),PCNT(6),TINFLO,TREL
C
C       format statements to provide for elevation data
C
1520     FORMAT(6X,'Keechelus',3X,f7.2,3x,F8.0,4X,F8.0,7X,F4.0,4X,F7.0,4X,F6.0)
1530     FORMAT(6X,'Kachess',5X,f7.2,3x,F8.0,4X,F8.0,7X,F4.0,4X,F7.0,4X,F6.0)
1540     FORMAT(6X,'Cle Elum',4X,f7.2,3x,F8.0,4X,F8.0,7X,F4.0,4X,F7.0,4X,F6.0)
1550     FORMAT(6X,'Bumping',5X,f7.2,3x,F8.0,4X,F8.0,7X,F4.0,4X,F7.0,4X,F6.0)
1560     FORMAT(6X,'Rimrock',5X,f7.2,3x,F8.0,4X,F8.0,7X,F4.0,4X,F7.0,4X,F6.0)
1565     Format(6X,'Clear Cr',4X,F7.2)
1570     FORMAT(6X,'TOTALS',16X,F8.0,4X,F8.0,7X,F4.0,4X,F7.0,4X,F6.0)
C  **************************************************************************
C               OUTPUT RIVERS AND CANALS INFORMATION
C----------------------------------------------------------------------------
C
        WRITE(IO2,1580)                  ! header for diversions and flows
        WRITE(IO2,1590)QC(1,2)!,Q(6,2)    ! Kittitas,
        WRITE(IO2,1610)QC(7,2),Q(6,2)    ! Roza,         EASW=6
        WRITE(IO2,1620)QC(6,2),Q(12,2)   ! Yakima-Tieton,TNAW=12
        WRITE(IO2,1640)QC(5,2),Q(13,2)   ! Wapato,       YRWW=13
        WRITE(IO2,1650)QC(4,2),Q(8,2)    ! Sunnyside,    UMTW=8
C
        TOTALD=NOVAL
        TDIV=NOVAL
        TDIV=TDIV+(QC(1,2)+QC(4,2)+QC(5,2)+QC(6,2)+QC(7,2))
        TOTALD=TDIV+DOU
C       dou (oth abv parker) is manually entered from keyboard input
C       note that as in tributary flows computation above,
C          westside and naches-selah are NOT included in totals
C
        WRITE(IO2,1660)TDIV,Q(16,2)      ! major usrs,   RBDW=16
        WRITE(IO2,1665)Q(14,2)           !               CLFW=14
        WRITE(IO2,1670)QC(8,2),Q(15,2)   ! westside,     TICW=15
        WRITE(IO2,1675)QC(3,2),Q(9,2)    ! naches-selah, NACW=9
        WRITE(IO2,1677)DOU,Q(10,2)       ! oth abv prkr, PARW=10
        WRITE(IO2,1850)Q(11,2)           !               YRPW=11
        WRITE(IO2,1687)TOTALD
        WRITE(IO2,1690)QC(9,2)           ! kennewick
        WRITE(IO2,1613)
        WRITE(IO2,1700)QC(2,2),QC(10,2),QC(11,2) !wopw,rzcw,chcw
        WRITE(IO2,1710)TRIFLO
        WRITE(IO2,1740)
        WRITE(IO2,1760)(COMMENTS(K),K=1,10)
C
C       report output format lines follow
C
1580     FORMAT(1X,/,6X,'IRRIGATION DIVERSIONS',20X,'RIVER FLOWS'/,6X,21('-'),4X
,'CFS',13X,11('-'),18X,'CFS')
c1590     FORMAT(6X,'Kittitas',15X,F5.0,13X,'Yakima River at Easton',4X,F7.0)
1590     FORMAT(6X,'Kittitas',15X,F5.0) !,13X,'Yakima River at Easton',4X,F7.0)
c1610     FORMAT(6X,'Roza',19X,F5.0) !,13X,'Yakima River at Cle Elum',2x,f7.0)
1610     FORMAT(6X,'Roza',19X,F5.0,13X,'Yakima River at Easton',4X,F7.0)
1613     FORMAT(/6X,'OTHER CANAL DIVERSIONS')
1620     FORMAT(6X,'Yakima-Tieton',10X,F5.0,13X,'Teanaway River at Forks',3X,F7.
0)
1640     FORMAT(6X,'Wapato',17X,F5.0,13X,'Yakima River near Horlick',1X,F7.0)
1650     FORMAT(6X,'Sunnyside',14X,F5.0,13X,'Yakima River near Umtanum',1X,F7.0)
1660     FORMAT(6X,'MAJOR USERS TOTAL',4X,F7.0,13X,'Yakima River blw Roza Dam',1
X,F7.0)
1665     FORMAT(47X,'Naches River nr. Clf''Dell',1x,F7.0)
1670     FORMAT(6X,'Westside',15X,F5.0,13X,'Tieton Rvr belw Cnl Hdwks',1X,F7.0)
1675     FORMAT(6X,'Naches-Selah',11X,F5.0,13X,'Naches River near Naches',2X,F7.
0)
1677     FORMAT(6X,'OTHERS ABOVE PARKER',2X,F7.0,13X,'Yakima River near Parker',
2X,F7.0)
1687     FORMAT(6X,'TOTAL ABOVE PARKER',3X,F7.0,/6X,28('-'))
1690     FORMAT(6X,'Kennewick',14X,F5.0)
1700     FORMAT(6X,'Wapatox',16X,F5.0,/,6X,'Roza at Headworks',6X,F5.0,/,6X,'Cha
ndler',15X,F5.0)
1710     FORMAT(/6X,'UNREGULATED TRIBUTARY & RETURN FLOW ABOVE PARKER',4X,2(' -
'),1F7.0,' CFS')
1740     FORMAT(/6X,'OPERATIONAL COMMENTS:',/,6X,21('-'))
1760     FORMAT(15(6X,A80/))
1850     FORMAT(47X,'Yakima River near Prosser',1X,F7.0)
1999     FORMAT(/6X,'SHORT RANGE OPERATING PLANS:',/,6X,29('-'))
C
C
1000    CALL EXIT
        END