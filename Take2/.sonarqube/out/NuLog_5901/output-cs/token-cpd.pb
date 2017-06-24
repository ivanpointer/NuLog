�
DC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Configuration\Config.cs
	namespace 	
NuLog
 
. 
Configuration 
{ 
public 

class 
Config 
{ 
public 
ICollection 
< 

RuleConfig %
>% &
Rules' ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
ICollection 
< 
TagGroupConfig )
>) *
	TagGroups+ 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public 
ICollection 
< 
TargetConfig '
>' (
Targets) 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public   
IDictionary   
<   
string   !
,  ! "
string  # )
>  ) *
MetaData  + 3
{  4 5
get  6 9
;  9 :
set  ; >
;  > ?
}  @ A
public&& 
bool&& 
IncludeStackFrame&& %
{&&& '
get&&( +
;&&+ ,
set&&- 0
;&&0 1
}&&2 3
public-- 
string-- 
FallbackLogPath-- %
{--& '
get--( +
;--+ ,
set--- 0
;--0 1
}--2 3
}.. 
}// �	
HC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Configuration\RuleConfig.cs
	namespace 	
NuLog
 
. 
Configuration 
{ 
public 

class 

RuleConfig 
{ 
public 
ICollection 
< 
string !
>! "
Includes# +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
ICollection 
< 
string !
>! "
Excludes# +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
ICollection 
< 
string !
>! "
Targets# *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public   
bool   
StrictInclude   !
{  " #
get  $ '
;  ' (
set  ) ,
;  , -
}  . /
public%% 
bool%% 
Final%% 
{%% 
get%% 
;%%  
set%%! $
;%%$ %
}%%& '
}&& 
}'' �
LC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Configuration\TagGroupConfig.cs
	namespace 	
NuLog
 
. 
Configuration 
{ 
public 

class 
TagGroupConfig 
{ 
public 
string 
BaseTag 
{ 
get  #
;# $
set% (
;( )
}* +
public 
ICollection 
< 
string !
>! "
Aliases# *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
} 
} �
JC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Configuration\TargetConfig.cs
	namespace 	
NuLog
 
. 
Configuration 
{ 
public 

class 
TargetConfig 
{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
Type 
{ 
get  
;  !
set" %
;% &
}' (
public 
IDictionary 
< 
string !
,! "
object# )
>) *

Properties+ 5
{6 7
get8 ;
;; <
set= @
;@ A
}B C
} 
} �
ZC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Configuration\ConfigurationManagerProvider.cs
	namespace 	
NuLog
 
. 
Configuration 
{		 
public 

class (
ConfigurationManagerProvider -
:. /"
IConfigurationProvider0 F
{ 
private 
readonly 
string 
sectionName  +
;+ ,
public (
ConfigurationManagerProvider +
(+ ,
string, 2
sectionName3 >
=? @
$strA H
)H I
{ 	
this 
. 
sectionName 
= 
sectionName *
;* +
} 	
public 
Config 
GetConfiguration &
(& '
)' (
{ 	
var 

xmlElement 
= 
( 

XmlElement (
)( ) 
ConfigurationManager) =
.= >

GetSection> H
(H I
thisI M
.M N
sectionNameN Y
)Y Z
;Z [
var 
xmlProvider 
= 
new !$
XmlConfigurationProvider" :
(: ;

xmlElement; E
)E F
;F G
var   
config   
=   
xmlProvider   $
.  $ %
GetConfiguration  % 5
(  5 6
)  6 7
;  7 8
return## 
config## 
;## 
}$$ 	
}%% 
}&& �
TC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Configuration\IConfigurationProvider.cs
	namespace 	
NuLog
 
. 
Configuration 
{ 
public

 

	interface

 "
IConfigurationProvider

 +
{ 
Config 
GetConfiguration 
(  
)  !
;! "
} 
} �~
VC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Configuration\XmlConfigurationProvider.cs
	namespace		 	
NuLog		
 
.		 
Configuration		 
{

 
public 

class $
XmlConfigurationProvider )
:* +"
IConfigurationProvider, B
{ 
private 
readonly 

XmlElement #

xmlElement$ .
;. /
public $
XmlConfigurationProvider '
(' (

XmlElement( 2

xmlElement3 =
)= >
{ 	
this 
. 

xmlElement 
= 

xmlElement (
;( )
} 	
public 
Config 
GetConfiguration &
(& '
)' (
{ 	
var 
config 
= 
new 
Config #
{ 
Rules   
=   

ParseRules   "
(  " #

xmlElement  # -
)  - .
,  . /
	TagGroups!! 
=!! 
ParseTagGroups!! *
(!!* +

xmlElement!!+ 5
)!!5 6
,!!6 7
Targets"" 
="" 
ParseTargets"" &
(""& '

xmlElement""' 1
)""1 2
,""2 3
MetaData## 
=## 
ParseMetaData## (
(##( )

xmlElement##) 3
)##3 4
}$$ 
;$$ 
config'' 
.'' 
IncludeStackFrame'' $
=''% &
GetBooleanAttribute''' :
('': ;

xmlElement''; E
,''E F
$str''G Z
)''Z [
;''[ \
config** 
.** 
FallbackLogPath** "
=**# $
GetStringAttribute**% 7
(**7 8

xmlElement**8 B
,**B C
$str**D Q
)**Q R
;**R S
return-- 
config-- 
;-- 
}.. 	
private55 
static55 
ICollection55 "
<55" #

RuleConfig55# -
>55- .

ParseRules55/ 9
(559 :

XmlElement55: D

xmlElement55E O
)55O P
{66 	
var88 
rules88 
=88 
new88 
List88  
<88  !

RuleConfig88! +
>88+ ,
(88, -
)88- .
;88. /
var;; 
rulesElement;; 
=;; 

xmlElement;; )
.;;) *
SelectSingleNode;;* :
(;;: ;
$str;;; B
);;B C
;;;C D
if>> 
(>> 
rulesElement>> 
==>> 
null>>  $
)>>$ %
{?? 
return@@ 
rules@@ 
;@@ 
}AA 
foreachDD 
(DD 
varDD 
ruleElementDD $
inDD% '
rulesElementDD( 4
.DD4 5
SelectNodesDD5 @
(DD@ A
$strDDA G
)DDG H
)DDH I
{EE 
rulesFF 
.FF 
AddFF 
(FF 
	ParseRuleFF #
(FF# $
(FF$ %

XmlElementFF% /
)FF/ 0
ruleElementFF0 ;
)FF; <
)FF< =
;FF= >
}GG 
returnJJ 
rulesJJ 
;JJ 
}KK 	
privatePP 
staticPP 

RuleConfigPP !
	ParseRulePP" +
(PP+ ,

XmlElementPP, 6

xmlElementPP7 A
)PPA B
{QQ 	
returnSS 
newSS 

RuleConfigSS !
{TT 
IncludesUU 
=UU 
GetAttributeListUU +
(UU+ ,

xmlElementUU, 6
,UU6 7
$strUU8 A
)UUA B
,UUB C
ExcludesVV 
=VV 
GetAttributeListVV +
(VV+ ,

xmlElementVV, 6
,VV6 7
$strVV8 A
)VVA B
,VVB C
TargetsWW 
=WW 
GetAttributeListWW *
(WW* +

xmlElementWW+ 5
,WW5 6
$strWW7 @
)WW@ A
,WWA B
StrictIncludeXX 
=XX 
GetBooleanAttributeXX  3
(XX3 4

xmlElementXX4 >
,XX> ?
$strXX@ O
)XXO P
,XXP Q
FinalYY 
=YY 
GetBooleanAttributeYY +
(YY+ ,

xmlElementYY, 6
,YY6 7
$strYY8 ?
)YY? @
}ZZ 
;ZZ 
}[[ 	
privatedd 
staticdd 
ICollectiondd "
<dd" #
TargetConfigdd# /
>dd/ 0
ParseTargetsdd1 =
(dd= >

XmlElementdd> H

xmlElementddI S
)ddS T
{ee 	
vargg 
targetsgg 
=gg 
newgg 
Listgg "
<gg" #
TargetConfiggg# /
>gg/ 0
(gg0 1
)gg1 2
;gg2 3
varjj 
targetsElementjj 
=jj  

xmlElementjj! +
.jj+ ,
SelectSingleNodejj, <
(jj< =
$strjj= F
)jjF G
;jjG H
ifmm 
(mm 
targetsElementmm 
==mm !
nullmm" &
)mm& '
{nn 
returnoo 
targetsoo 
;oo 
}pp 
foreachss 
(ss 
varss 
targetElementss &
inss' )
targetsElementss* 8
.ss8 9
SelectNodesss9 D
(ssD E
$strssE M
)ssM N
)ssN O
{tt 
targetsuu 
.uu 
Adduu 
(uu 
ParseTargetuu '
(uu' (
(uu( )

XmlElementuu) 3
)uu3 4
targetElementuu4 A
)uuA B
)uuB C
;uuC D
}vv 
returnyy 
targetsyy 
;yy 
}zz 	
private 
static 
TargetConfig #
ParseTarget$ /
(/ 0

XmlElement0 :

xmlElement; E
)E F
{
�� 	
return
�� 
new
�� 
TargetConfig
�� #
{
�� 
Name
�� 
=
��  
GetStringAttribute
�� )
(
��) *

xmlElement
��* 4
,
��4 5
$str
��6 <
)
��< =
,
��= >
Type
�� 
=
��  
GetStringAttribute
�� )
(
��) *

xmlElement
��* 4
,
��4 5
$str
��6 <
)
��< =
,
��= >

Properties
�� 
=
�� '
GetAttributesAsDictionary
�� 6
(
��6 7

xmlElement
��7 A
)
��A B
}
�� 
;
�� 
}
�� 	
private
�� 
static
�� 
ICollection
�� "
<
��" #
TagGroupConfig
��# 1
>
��1 2
ParseTagGroups
��3 A
(
��A B

XmlElement
��B L

xmlElement
��M W
)
��W X
{
�� 	
var
�� 
	tagGroups
�� 
=
�� 
new
�� 
List
��  $
<
��$ %
TagGroupConfig
��% 3
>
��3 4
(
��4 5
)
��5 6
;
��6 7
var
�� 
tagGroupsElement
��  
=
��! "

xmlElement
��# -
.
��- .
SelectSingleNode
��. >
(
��> ?
$str
��? J
)
��J K
;
��K L
if
�� 
(
�� 
tagGroupsElement
��  
==
��! #
null
��$ (
)
��( )
{
�� 
return
�� 
	tagGroups
��  
;
��  !
}
�� 
foreach
�� 
(
�� 
var
�� 
tagGroupElement
�� (
in
��) +
tagGroupsElement
��, <
.
��< =
SelectNodes
��= H
(
��H I
$str
��I P
)
��P Q
)
��Q R
{
�� 
	tagGroups
�� 
.
�� 
Add
�� 
(
�� 
ParseTagGroup
�� +
(
��+ ,
(
��, -

XmlElement
��- 7
)
��7 8
tagGroupElement
��8 G
)
��G H
)
��H I
;
��I J
}
�� 
return
�� 
	tagGroups
�� 
;
�� 
}
�� 	
private
�� 
static
�� 
TagGroupConfig
�� %
ParseTagGroup
��& 3
(
��3 4

XmlElement
��4 >

xmlElement
��? I
)
��I J
{
�� 	
return
�� 
new
�� 
TagGroupConfig
�� %
{
�� 
BaseTag
�� 
=
��  
GetStringAttribute
�� ,
(
��, -

xmlElement
��- 7
,
��7 8
$str
��9 B
)
��B C
,
��C D
Aliases
�� 
=
�� 
GetAttributeList
�� *
(
��* +

xmlElement
��+ 5
,
��5 6
$str
��7 @
)
��@ A
}
�� 
;
�� 
}
�� 	
private
�� 
static
�� 
IDictionary
�� "
<
��" #
string
��# )
,
��) *
string
��+ 1
>
��1 2
ParseMetaData
��3 @
(
��@ A

XmlElement
��A K

xmlElement
��L V
)
��V W
{
�� 	
var
�� 
metaData
�� 
=
�� 
new
�� 

Dictionary
�� )
<
��) *
string
��* 0
,
��0 1
string
��2 8
>
��8 9
(
��9 :
)
��: ;
;
��; <
var
�� 
metaDataElement
�� 
=
��  !

xmlElement
��" ,
.
��, -
SelectSingleNode
��- =
(
��= >
$str
��> H
)
��H I
;
��I J
if
�� 
(
�� 
metaDataElement
�� 
==
��  "
null
��# '
)
��' (
{
�� 
return
�� 
metaData
�� 
;
��  
}
�� 
foreach
�� 
(
�� 
var
�� 
metaDataEntry
�� &
in
��' )
metaDataElement
��* 9
.
��9 :
SelectNodes
��: E
(
��E F
$str
��F K
)
��K L
)
��L M
{
�� 
var
�� 
key
�� 
=
��  
GetStringAttribute
�� ,
(
��, -
(
��- .

XmlElement
��. 8
)
��8 9
metaDataEntry
��9 F
,
��F G
$str
��H M
)
��M N
;
��N O
var
�� 
value
�� 
=
��  
GetStringAttribute
�� .
(
��. /
(
��/ 0

XmlElement
��0 :
)
��: ;
metaDataEntry
��; H
,
��H I
$str
��J Q
)
��Q R
;
��R S
metaData
�� 
[
�� 
key
�� 
]
�� 
=
�� 
value
��  %
;
��% &
}
�� 
return
�� 
metaData
�� 
;
�� 
}
�� 	
private
�� 
static
�� 
ICollection
�� "
<
��" #
string
��# )
>
��) *
GetAttributeList
��+ ;
(
��; <

XmlElement
��< F

xmlElement
��G Q
,
��Q R
string
��S Y
attributeName
��Z g
)
��g h
{
�� 	
ICollection
�� 
<
�� 
string
�� 
>
�� 
items
��  %
=
��& '
null
��( ,
;
��, -
var
�� 
	attribute
�� 
=
�� 

xmlElement
�� &
.
��& '

Attributes
��' 1
.
��1 2
GetNamedItem
��2 >
(
��> ?
attributeName
��? L
)
��L M
;
��M N
if
�� 
(
�� 
	attribute
�� 
!=
�� 
null
�� !
)
��! "
{
�� 
var
�� 
attributeValue
�� "
=
��# $
	attribute
��% .
.
��. /
Value
��/ 4
;
��4 5
var
�� 
attributeArray
�� "
=
��# $
attributeValue
��% 3
.
��3 4
Split
��4 9
(
��9 :
$char
��: =
)
��= >
;
��> ?
items
�� 
=
�� 
attributeArray
�� &
.
��& '
ToList
��' -
(
��- .
)
��. /
;
��/ 0
}
�� 
return
�� 
items
�� 
??
�� 
new
�� 
List
��  $
<
��$ %
string
��% +
>
��+ ,
(
��, -
)
��- .
;
��. /
}
�� 	
private
�� 
static
�� 
bool
�� !
GetBooleanAttribute
�� /
(
��/ 0

XmlElement
��0 :

xmlElement
��; E
,
��E F
string
��G M
attributeName
��N [
,
��[ \
bool
��] a
defaultValue
��b n
=
��o p
false
��q v
)
��v w
{
�� 	
var
�� 
	attribute
�� 
=
�� 

xmlElement
�� &
.
��& '

Attributes
��' 1
.
��1 2
GetNamedItem
��2 >
(
��> ?
attributeName
��? L
)
��L M
;
��M N
if
�� 
(
�� 
	attribute
�� 
==
�� 
null
�� !
)
��! "
{
�� 
return
�� 
defaultValue
�� #
;
��# $
}
�� 
var
�� 
attributeValue
�� 
=
��  
	attribute
��! *
.
��* +
Value
��+ 0
;
��0 1
bool
�� 
parsed
�� 
=
�� 
false
�� 
;
��  
if
�� 
(
�� 
bool
�� 
.
�� 
TryParse
�� 
(
�� 
attributeValue
�� ,
,
��, -
out
��. 1
parsed
��2 8
)
��8 9
)
��9 :
{
�� 
return
�� 
parsed
�� 
;
�� 
}
�� 
else
�� 
{
�� 
return
�� 
defaultValue
�� #
;
��# $
}
�� 
}
�� 	
private
�� 
static
�� 
string
��  
GetStringAttribute
�� 0
(
��0 1

XmlElement
��1 ;

xmlElement
��< F
,
��F G
string
��H N
attributeName
��O \
)
��\ ]
{
�� 	
var
�� 
	attribute
�� 
=
�� 

xmlElement
�� &
.
��& '

Attributes
��' 1
.
��1 2
GetNamedItem
��2 >
(
��> ?
attributeName
��? L
)
��L M
;
��M N
return
�� 
	attribute
�� 
!=
�� 
null
��  $
?
��% &
	attribute
��' 0
.
��0 1
Value
��1 6
:
��7 8
null
��9 =
;
��= >
}
�� 	
private
�� 
static
�� 
IDictionary
�� "
<
��" #
string
��# )
,
��) *
object
��+ 1
>
��1 2'
GetAttributesAsDictionary
��3 L
(
��L M

XmlElement
��M W

xmlElement
��X b
)
��b c
{
�� 	
var
�� 
dict
�� 
=
�� 
new
�� 

Dictionary
�� %
<
��% &
string
��& ,
,
��, -
object
��. 4
>
��4 5
(
��5 6
)
��6 7
;
��7 8
foreach
�� 
(
�� 
var
�� 
	attribute
�� "
in
��# %

xmlElement
��& 0
.
��0 1

Attributes
��1 ;
)
��; <
{
�� 
var
�� 
attr
�� 
=
�� 
(
�� 
XmlAttribute
�� (
)
��( )
	attribute
��) 2
;
��2 3
dict
�� 
[
�� 
attr
�� 
.
�� 
Name
�� 
]
�� 
=
��  !
attr
��" &
.
��& '
Value
��' ,
;
��, -
}
�� 
return
�� 
dict
�� 
;
�� 
}
�� 	
}
�� 
}�� �
KC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Dispatchers\IFallbackLogger.cs
	namespace 	
NuLog
 
. 
Dispatchers 
{ 
public 

	interface 
IFallbackLogger $
{ 
void 
Log 
( 
	Exception 
	exception $
,$ %
ITarget& -
target. 4
,4 5
	ILogEvent6 ?
logEvent@ H
)H I
;I J
void 
Log 
( 
string 
message 
,  
params! '
object( .
[. /
]/ 0
args1 5
)5 6
;6 7
} 
} �F
NC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Dispatchers\StandardDispatcher.cs
	namespace 	
NuLog
 
. 
Dispatchers 
{ 
public 

class 
StandardDispatcher #
:$ %
IDispatcher& 1
{ 
private 
readonly 
IEnumerable $
<$ %
ITarget% ,
>, -
targets. 5
;5 6
private"" 
readonly"" 

ITagRouter"" #
	tagRouter""$ -
;""- .
private'' 
readonly'' 
IFallbackLogger'' (
fallbackLogger'') 7
;''7 8
private// 
readonly// 
ConcurrentQueue// (
<//( )
	ILogEvent//) 2
>//2 3
logEventQueue//4 A
;//A B
private55 
Timer55 
logEventQueueTimer55 (
;55( )
private:: 
bool:: 
isDisposing::  
;::  !
public@@ 
StandardDispatcher@@ !
(@@! "
IEnumerable@@" -
<@@- .
ITarget@@. 5
>@@5 6
targets@@7 >
,@@> ?

ITagRouter@@@ J
	tagRouter@@K T
,@@T U
IFallbackLogger@@V e
fallbackLogger@@f t
)@@t u
{AA 	
thisBB 
.BB 
targetsBB 
=BB 
targetsBB "
;BB" #
thisDD 
.DD 
	tagRouterDD 
=DD 
	tagRouterDD &
;DD& '
thisFF 
.FF 
fallbackLoggerFF 
=FF  !
fallbackLoggerFF" 0
;FF0 1
thisKK 
.KK 
logEventQueueKK 
=KK  
newKK! $
ConcurrentQueueKK% 4
<KK4 5
	ILogEventKK5 >
>KK> ?
(KK? @
)KK@ A
;KKA B
thisNN 
.NN 
logEventQueueTimerNN #
=NN$ %
newNN& )
TimerNN* /
(NN/ 0"
OnLogQueueTimerElapsedNN0 F
,NNF G
thisNNH L
,NNL M
$numNNN Q
,NNQ R
$numNNS V
)NNV W
;NNW X
}OO 	
publicQQ 
voidQQ 
DispatchNowQQ 
(QQ  
	ILogEventQQ  )
logEventQQ* 2
)QQ2 3
{RR 	
trySS 
{TT 
varVV 
targetNamesVV 
=VV  !
	tagRouterVV" +
.VV+ ,
RouteVV, 1
(VV1 2
logEventVV2 :
.VV: ;
TagsVV; ?
)VV? @
;VV@ A
foreachYY 
(YY 
varYY 

targetNameYY '
inYY( *
targetNamesYY+ 6
)YY6 7
{ZZ 
var\\ 
target\\ 
=\\  
targets\\! (
.\\( )
FirstOrDefault\\) 7
(\\7 8
t\\8 9
=>\\: <
string\\= C
.\\C D
Equals\\D J
(\\J K

targetName\\K U
,\\U V
t\\W X
.\\X Y
Name\\Y ]
,\\] ^
StringComparison\\_ o
.\\o p
OrdinalIgnoreCase	\\p �
)
\\� �
)
\\� �
;
\\� �
if]] 
(]] 
target]] 
!=]] !
null]]" &
)]]& '
{^^ 
try__ 
{`` 
logEventbb $
.bb$ %
WriteTobb% ,
(bb, -
targetbb- 3
)bb3 4
;bb4 5
}cc 
catchdd 
(dd 
	Exceptiondd (
causedd) .
)dd. /
{ee 
FallbackLoggg '
(gg' (
causegg( -
,gg- .
targetgg/ 5
,gg5 6
logEventgg7 ?
)gg? @
;gg@ A
}hh 
}ii 
}jj 
}kk 
catchll 
(ll 
	Exceptionll 
causell "
)ll" #
{mm 
FallbackLogoo 
(oo 
$stroo @
,oo@ A
causeooB G
)ooG H
;ooH I
}pp 
}qq 	
privatess 
voidss 
FallbackLogss  
(ss  !
stringss! '
messagess( /
,ss/ 0
paramsss1 7
objectss8 >
[ss> ?
]ss? @
argsssA E
)ssE F
{tt 	
tryuu 
{vv 
thisww 
.ww 
fallbackLoggerww #
.ww# $
Logww$ '
(ww' (
messageww( /
,ww/ 0
argsww1 5
)ww5 6
;ww6 7
}xx 
catchyy 
(yy 
	Exceptionyy 
causeyy "
)yy" #
{zz 
Trace{{ 
.{{ 

TraceError{{  
({{  !
$str{{! \
,{{\ ]
cause{{^ c
){{c d
;{{d e
}|| 
}}} 	
private 
void 
FallbackLog  
(  !
	Exception! *
	exception+ 4
,4 5
ITarget6 =
target> D
,D E
	ILogEventF O
logEventP X
)X Y
{
�� 	
try
�� 
{
�� 
this
�� 
.
�� 
fallbackLogger
�� #
.
��# $
Log
��$ '
(
��' (
	exception
��( 1
,
��1 2
target
��3 9
,
��9 :
logEvent
��; C
)
��C D
;
��D E
}
�� 
catch
�� 
(
�� 
	Exception
�� 
cause
�� "
)
��" #
{
�� 
Trace
�� 
.
�� 

TraceError
��  
(
��  !
$str
��! o
,
��o p
cause
��q v
,
��v w
	exception��x �
)��� �
;��� �
}
�� 
}
�� 	
public
�� 
void
��  
EnqueueForDispatch
�� &
(
��& '
	ILogEvent
��' 0
logEvent
��1 9
)
��9 :
{
�� 	
if
�� 
(
�� 
isDisposing
�� 
)
�� 
{
�� 
throw
�� 
new
�� '
InvalidOperationException
�� 3
(
��3 4
$str
��4 {
)
��{ |
;
��| }
}
�� 
logEventQueue
�� 
.
�� 
Enqueue
�� !
(
��! "
logEvent
��" *
)
��* +
;
��+ ,
}
�� 	
public
�� 
void
�� 
Dispose
�� 
(
�� 
)
�� 
{
�� 	
Dispose
�� 
(
�� 
true
�� 
)
�� 
;
�� 
GC
�� 
.
�� 
SuppressFinalize
�� 
(
��  
this
��  $
)
��$ %
;
��% &
}
�� 	
	protected
�� 
virtual
�� 
void
�� 
Dispose
�� &
(
��& '
bool
��' +
	disposing
��, 5
)
��5 6
{
�� 	
if
�� 
(
�� 
	disposing
�� 
)
�� 
{
�� 
isDisposing
�� 
=
�� 
true
�� "
;
��" #
this
�� 
.
��  
logEventQueueTimer
�� '
.
��' (
Dispose
��( /
(
��/ 0
)
��0 1
;
��1 2
this
�� 
.
��  
logEventQueueTimer
�� '
=
��( )
null
��* .
;
��. /
}
�� 
ProcessLogQueue
�� 
(
�� 
)
�� 
;
�� 
foreach
�� 
(
�� 
var
�� 
target
�� 
in
��  "
this
��# '
.
��' (
targets
��( /
)
��/ 0
{
�� 
target
�� 
.
�� 
Dispose
�� 
(
�� 
)
��  
;
��  !
}
�� 
}
�� 	
private
�� 
static
�� 
void
�� $
OnLogQueueTimerElapsed
�� 2
(
��2 3
object
��3 9 
dispatcherInstance
��: L
)
��L M
{
�� 	
var
�� 

dispatcher
�� 
=
��  
dispatcherInstance
�� /
as
��0 2 
StandardDispatcher
��3 E
;
��E F

dispatcher
�� 
.
�� 
ProcessLogQueue
�� &
(
��& '
)
��' (
;
��( )
}
�� 	
	protected
�� 
void
�� 
ProcessLogQueue
�� &
(
��& '
)
��' (
{
�� 	
	ILogEvent
�� 
logEvent
�� 
;
�� 
while
�� 
(
�� 
this
�� 
.
�� 
logEventQueue
�� %
.
��% &

TryDequeue
��& 0
(
��0 1
out
��1 4
logEvent
��5 =
)
��= >
)
��> ?
{
�� 
DispatchNow
�� 
(
�� 
logEvent
�� $
)
��$ %
;
��% &
}
�� 
}
�� 	
}
�� 
}�� �
QC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Dispatchers\TagRouters\ITagRouter.cs
	namespace 	
NuLog
 
. 
Dispatchers 
. 

TagRouters &
{ 
public 

	interface 

ITagRouter 
{ 
IEnumerable 
< 
string 
> 
Route !
(! "
IEnumerable" -
<- .
string. 4
>4 5
tags6 :
): ;
;; <
} 
} �	
KC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Dispatchers\TagRouters\Rule.cs
	namespace 	
NuLog
 
. 
Dispatchers 
. 

TagRouters &
{ 
public 

class 
Rule 
{ 
public 
IEnumerable 
< 
string !
>! "
Include# *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
IEnumerable 
< 
string !
>! "
Exclude# *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
IEnumerable 
< 
string !
>! "
Targets# *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public%% 
bool%% 
StrictInclude%% !
{%%" #
get%%$ '
;%%' (
set%%) ,
;%%, -
}%%. /
public++ 
bool++ 
Final++ 
{++ 
get++ 
;++  
set++! $
;++$ %
}++& '
},, 
}-- �
OC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Dispatchers\TagRouters\TagGroup.cs
	namespace 	
NuLog
 
. 
Dispatchers 
. 

TagRouters &
{ 
public 

class 
TagGroup 
{ 
public 
string 
BaseTag 
{ 
get  #
;# $
set% (
;( )
}* +
public 
ICollection 
< 
string !
>! "
Aliases# *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
} 
} ɮ
OC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Factories\StandardLoggerFactory.cs
	namespace 	
NuLog
 
. 
	Factories 
{ 
public 

class !
StandardLoggerFactory &
:' (
ILoggerFactory) 7
,7 8
ILayoutFactory9 G
{ 
public"" 
const"" 
string"" 
DefaultLayoutFormat"" /
=""0 1
$str	""2 �
;
""� �
private(( 
static(( 
readonly(( 
Type((  $
ILayoutTargetType((% 6
=((7 8
typeof((9 ?
(((? @
ILayoutTarget((@ M
)((M N
;((N O
	protected-- 
readonly-- 
Config-- !
Config--" (
;--( )
	protected22 
readonly22 
IFallbackLogger22 *
FallbackLogger22+ 9
;229 :
private88 
static88 
readonly88 
object88  &
FactoryLock88' 2
=883 4
new885 8
object889 ?
(88? @
)88@ A
;88A B
private== 
IDispatcher== 
_dispatcher== '
;==' (
	protectedBB 
IDispatcherBB 

DispatcherBB (
{CC 	
getDD 
{EE 
ifFF 
(FF 
_dispatcherFF 
==FF  "
nullFF# '
)FF' (
{GG 
lockHH 
(HH 
FactoryLockHH %
)HH% &
{II 
ifJJ 
(JJ 
isDisposingJJ '
)JJ' (
{KK 
throwLL !
newLL" %%
InvalidOperationExceptionLL& ?
(LL? @
$strLL@ z
)LLz {
;LL{ |
}MM 
elseNN 
ifNN 
(NN  !
_dispatcherNN! ,
==NN- /
nullNN0 4
)NN4 5
{OO 
_dispatcherPP '
=PP( )
GetDispatcherPP* 7
(PP7 8
)PP8 9
;PP9 :
}QQ 
}RR 
}SS 
returnTT 
_dispatcherTT "
;TT" #
}UU 
}VV 	
private[[ 
ITagNormalizer[[ 
_tagNormalizer[[ -
;[[- .
	protected`` 
ITagNormalizer``  
TagNormalizer``! .
{aa 	
getbb 
{cc 
ifdd 
(dd 
_tagNormalizerdd "
==dd# %
nulldd& *
)dd* +
{ee 
lockff 
(ff 
FactoryLockff %
)ff% &
{gg 
ifhh 
(hh 
_tagNormalizerhh *
==hh+ -
nullhh. 2
)hh2 3
{ii 
_tagNormalizerjj *
=jj+ ,
GetTagNormalizerjj- =
(jj= >
)jj> ?
;jj? @
}kk 
}ll 
}mm 
returnnn 
_tagNormalizernn %
;nn% &
}oo 
}pp 	
privateuu 
IDictionaryuu 
<uu 
stringuu "
,uu" #
objectuu$ *
>uu* +
_defaultMetaDatauu, <
;uu< =
	protectedzz 
IDictionaryzz 
<zz 
stringzz $
,zz$ %
objectzz& ,
>zz, -
DefaultMetaDatazz. =
{{{ 	
get|| 
{}} 
if~~ 
(~~ 
_defaultMetaData~~ $
==~~% '
null~~( ,
)~~, -
{ 
lock
�� 
(
�� 
FactoryLock
�� %
)
��% &
{
�� 
if
�� 
(
�� 
_defaultMetaData
�� ,
==
��- /
null
��0 4
)
��4 5
{
�� 
_defaultMetaData
�� ,
=
��- .
new
��/ 2 
ReadOnlyDictionary
��3 E
<
��E F
string
��F L
,
��L M
object
��N T
>
��T U
(
��U V

ToMetaData
��V `
(
��` a
Config
��a g
.
��g h
MetaData
��h p
)
��p q
)
��q r
;
��r s
}
�� 
}
�� 
}
�� 
return
�� 
_defaultMetaData
�� '
;
��' (
}
�� 
}
�� 	
private
�� 
bool
�� 
isDisposing
��  
;
��  !
public
�� #
StandardLoggerFactory
�� $
(
��$ %
Config
��% +
config
��, 2
)
��2 3
{
�� 	
Config
�� 
=
�� 
config
�� 
;
�� 
try
�� 
{
�� 
FallbackLogger
�� 
=
��  
GetFallbackLogger
��! 2
(
��2 3
)
��3 4
;
��4 5
}
�� 
catch
�� 
(
�� 
	Exception
�� 
cause
�� "
)
��" #
{
�� 
FallbackLogger
�� 
=
��  
new
��! $)
StandardTraceFallbackLogger
��% @
(
��@ A
)
��A B
;
��B C
FallbackLogger
�� 
.
�� 
Log
�� "
(
��" #
$str
��# Q
,
��Q R
cause
��S X
)
��X Y
;
��Y Z
}
�� 
}
�� 	
public
�� 
ILogger
�� 
	GetLogger
��  
(
��  !
IMetaDataProvider
��! 2
metaDataProvider
��3 C
,
��C D
IEnumerable
��E P
<
��P Q
string
��Q W
>
��W X
defaultTags
��Y d
)
��d e
{
�� 	
return
�� 
new
�� 
StandardLogger
�� %
(
��% &

Dispatcher
��& 0
,
��0 1
TagNormalizer
��2 ?
,
��? @
metaDataProvider
��A Q
,
��Q R
defaultTags
��S ^
,
��^ _
DefaultMetaData
��` o
,
��o p
Config
��q w
.
��w x 
IncludeStackFrame��x �
)��� �
;��� �
}
�� 	
public
�� 
virtual
�� 
IDispatcher
�� "
GetDispatcher
��# 0
(
��0 1
)
��1 2
{
�� 	
var
�� 
targets
�� 
=
�� 

GetTargets
�� $
(
��$ %
)
��% &
;
��& '
var
�� 
	tagRouter
�� 
=
�� 
GetTagRouter
�� (
(
��( )
)
��) *
;
��* +
return
�� 
new
��  
StandardDispatcher
�� )
(
��) *
targets
��* 1
,
��1 2
	tagRouter
��3 <
,
��< =
null
��> B
)
��B C
;
��C D
}
�� 	
public
�� 
virtual
�� 
ICollection
�� "
<
��" #
ITarget
��# *
>
��* +

GetTargets
��, 6
(
��6 7
)
��7 8
{
�� 	
var
�� 
targets
�� 
=
�� 
new
�� 
List
�� "
<
��" #
ITarget
��# *
>
��* +
(
��+ ,
)
��, -
;
��- .
if
�� 
(
�� 
Config
�� 
.
�� 
Targets
�� 
==
�� !
null
��" &
)
��& '
{
�� 
return
�� 
targets
�� 
;
�� 
}
�� 
foreach
�� 
(
�� 
var
�� 
targetConfig
�� %
in
��& (
Config
��) /
.
��/ 0
Targets
��0 7
)
��7 8
{
�� 
var
�� 
target
�� 
=
�� 
BuildTarget
�� (
(
��( )
targetConfig
��) 5
)
��5 6
;
��6 7
if
�� 
(
�� 
target
�� 
!=
�� 
null
�� "
)
��" #
{
�� 
targets
�� 
.
�� 
Add
�� 
(
��  
target
��  &
)
��& '
;
��' (
}
�� 
}
�� 
return
�� 
targets
�� 
;
�� 
}
�� 	
public
�� 
virtual
��  
ITagGroupProcessor
�� )"
GetTagGroupProcessor
��* >
(
��> ?
)
��? @
{
�� 	
return
�� 
new
�� '
StandardTagGroupProcessor
�� 0
(
��0 1
ToTagGroups
��1 <
(
��< =
Config
��= C
.
��C D
	TagGroups
��D M
)
��M N
)
��N O
;
��O P
}
�� 	
public
�� 
virtual
�� 
IRuleProcessor
�� %
GetRuleProcessor
��& 6
(
��6 7
)
��7 8
{
�� 	
var
�� 
tagGroupProcessor
�� !
=
��" #"
GetTagGroupProcessor
��$ 8
(
��8 9
)
��9 :
;
��: ;
return
�� 
new
�� #
StandardRuleProcessor
�� ,
(
��, -
ToRules
��- 4
(
��4 5
Config
��5 ;
.
��; <
Rules
��< A
)
��A B
,
��B C
tagGroupProcessor
��D U
)
��U V
;
��V W
}
�� 	
public
�� 
virtual
�� 

ITagRouter
�� !
GetTagRouter
��" .
(
��. /
)
��/ 0
{
�� 	
var
�� 
ruleProcessor
�� 
=
�� 
GetRuleProcessor
��  0
(
��0 1
)
��1 2
;
��2 3
return
�� 
new
�� 
StandardTagRouter
�� (
(
��( )
ruleProcessor
��) 6
)
��6 7
;
��7 8
}
�� 	
public
�� 
virtual
�� 
ITagNormalizer
�� %
GetTagNormalizer
��& 6
(
��6 7
)
��7 8
{
�� 	
return
�� 
new
�� #
StandardTagNormalizer
�� ,
(
��, -
)
��- .
;
��. /
}
�� 	
public
�� 
virtual
�� 
ILayoutParser
�� $
GetLayoutParser
��% 4
(
��4 5
)
��5 6
{
�� 	
return
�� 
new
�� "
StandardLayoutParser
�� +
(
��+ ,
)
��, -
;
��- .
}
�� 	
public
�� 
virtual
�� 
IPropertyParser
�� &
GetPropertyParser
��' 8
(
��8 9
)
��9 :
{
�� 	
return
�� 
new
�� $
StandardPropertyParser
�� -
(
��- .
)
��. /
;
��/ 0
}
�� 	
public
�� 
virtual
�� 
ILayout
�� 
	GetLayout
�� (
(
��( )
string
��) /
format
��0 6
)
��6 7
{
�� 	
var
�� 
layoutParser
�� 
=
�� 
GetLayoutParser
�� .
(
��. /
)
��/ 0
;
��0 1
format
�� 
=
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� )
(
��) *
format
��* 0
)
��0 1
?
��2 3!
DefaultLayoutFormat
��4 G
:
��H I
format
��J P
;
��P Q
var
�� 
layoutParms
�� 
=
�� 
layoutParser
�� *
.
��* +
Parse
��+ 0
(
��0 1
format
��1 7
)
��7 8
;
��8 9
var
�� 
propertyParser
�� 
=
��  
GetPropertyParser
��! 2
(
��2 3
)
��3 4
;
��4 5
return
�� 
new
�� 
StandardLayout
�� %
(
��% &
layoutParms
��& 1
,
��1 2
propertyParser
��3 A
)
��A B
;
��B C
}
�� 	
public
�� 
virtual
�� 
IFallbackLogger
�� &
GetFallbackLogger
��' 8
(
��8 9
)
��9 :
{
�� 	
if
�� 
(
�� 
Config
�� 
==
�� 
null
�� 
||
�� !
string
��" (
.
��( )
IsNullOrEmpty
��) 6
(
��6 7
Config
��7 =
.
��= >
FallbackLogPath
��> M
)
��M N
)
��N O
{
�� 
return
�� 
new
�� )
StandardTraceFallbackLogger
�� 6
(
��6 7
)
��7 8
;
��8 9
}
�� 
else
�� 
{
�� 
return
�� 
new
�� (
StandardFileFallbackLogger
�� 5
(
��5 6
Config
��6 <
.
��< =
FallbackLogPath
��= L
)
��L M
;
��M N
}
�� 
}
�� 	
	protected
�� 
virtual
�� 
IEnumerable
�� %
<
��% &
TagGroup
��& .
>
��. /
ToTagGroups
��0 ;
(
��; <
IEnumerable
��< G
<
��G H
TagGroupConfig
��H V
>
��V W
tagGroupConfigs
��X g
)
��g h
{
�� 	
var
�� 
	tagGroups
�� 
=
�� 
new
�� 
List
��  $
<
��$ %
TagGroup
��% -
>
��- .
(
��. /
)
��/ 0
;
��0 1
if
�� 
(
�� 
tagGroupConfigs
�� 
==
��  "
null
��# '
)
��' (
{
�� 
return
�� 
	tagGroups
��  
;
��  !
}
�� 
foreach
�� 
(
�� 
var
�� 
config
�� 
in
��  "
tagGroupConfigs
��# 2
)
��2 3
{
�� 
	tagGroups
�� 
.
�� 
Add
�� 
(
�� 

ToTagGroup
�� (
(
��( )
config
��) /
)
��/ 0
)
��0 1
;
��1 2
}
�� 
return
�� 
	tagGroups
�� 
;
�� 
}
�� 	
	protected
�� 
virtual
�� 
TagGroup
�� "

ToTagGroup
��# -
(
��- .
TagGroupConfig
��. <
config
��= C
)
��C D
{
�� 	
return
�� 
new
�� 
TagGroup
�� 
{
�� 
BaseTag
�� 
=
�� 
config
��  
.
��  !
BaseTag
��! (
,
��( )
Aliases
�� 
=
�� 
config
��  
.
��  !
Aliases
��! (
}
�� 
;
�� 
}
�� 	
	protected
�� 
virtual
�� 
IEnumerable
�� %
<
��% &
Rule
��& *
>
��* +
ToRules
��, 3
(
��3 4
IEnumerable
��4 ?
<
��? @

RuleConfig
��@ J
>
��J K
ruleConfigs
��L W
)
��W X
{
�� 	
var
�� 
rules
�� 
=
�� 
new
�� 
List
��  
<
��  !
Rule
��! %
>
��% &
(
��& '
)
��' (
;
��( )
if
�� 
(
�� 
ruleConfigs
�� 
==
�� 
null
�� #
)
��# $
{
�� 
return
�� 
rules
�� 
;
�� 
}
�� 
foreach
�� 
(
�� 
var
�� 
config
�� 
in
��  "
ruleConfigs
��# .
)
��. /
{
�� 
rules
�� 
.
�� 
Add
�� 
(
�� 
ToRule
��  
(
��  !
config
��! '
)
��' (
)
��( )
;
��) *
}
�� 
return
�� 
rules
�� 
;
�� 
}
�� 	
	protected
�� 
virtual
�� 
Rule
�� 
ToRule
�� %
(
��% &

RuleConfig
��& 0
config
��1 7
)
��7 8
{
�� 	
return
�� 
new
�� 
Rule
�� 
{
�� 
Include
�� 
=
�� 
config
��  
.
��  !
Includes
��! )
,
��) *
StrictInclude
�� 
=
�� 
config
��  &
.
��& '
StrictInclude
��' 4
,
��4 5
Exclude
�� 
=
�� 
config
��  
.
��  !
Excludes
��! )
,
��) *
Targets
�� 
=
�� 
config
��  
.
��  !
Targets
��! (
,
��( )
Final
�� 
=
�� 
config
�� 
.
�� 
Final
�� $
}
�� 
;
�� 
}
�� 	
	protected
�� 
virtual
�� 
IDictionary
�� %
<
��% &
string
��& ,
,
��, -
object
��. 4
>
��4 5

ToMetaData
��6 @
(
��@ A
IDictionary
��A L
<
��L M
string
��M S
,
��S T
string
��U [
>
��[ \
configMetaData
��] k
)
��k l
{
�� 	
var
�� 
metaData
�� 
=
�� 
new
�� 

Dictionary
�� )
<
��) *
string
��* 0
,
��0 1
object
��2 8
>
��8 9
(
��9 :
)
��: ;
;
��; <
if
�� 
(
�� 
configMetaData
�� 
==
�� !
null
��" &
)
��& '
{
�� 
return
�� 
metaData
�� 
;
��  
}
�� 
foreach
�� 
(
�� 
var
�� 
entry
�� 
in
�� !
configMetaData
��" 0
)
��0 1
{
�� 
metaData
�� 
[
�� 
entry
�� 
.
�� 
Key
�� "
]
��" #
=
��$ %
entry
��& +
.
��+ ,
Value
��, 1
;
��1 2
}
�� 
return
�� 
metaData
�� 
;
�� 
}
�� 	
public
�� 
void
�� 
Dispose
�� 
(
�� 
)
�� 
{
�� 	
Dispose
�� 
(
�� 
true
�� 
)
�� 
;
�� 
GC
�� 
.
�� 
SuppressFinalize
�� 
(
��  
this
��  $
)
��$ %
;
��% &
}
�� 	
	protected
�� 
virtual
�� 
void
�� 
Dispose
�� &
(
��& '
bool
��' +
	disposing
��, 5
)
��5 6
{
�� 	
if
�� 
(
�� 
	disposing
�� 
)
�� 
{
�� 
isDisposing
�� 
=
�� 
true
�� "
;
��" #
}
�� 
if
�� 
(
�� 
_dispatcher
�� 
!=
�� 
null
�� #
)
��# $
{
�� 
_dispatcher
�� 
.
�� 
Dispose
�� #
(
��# $
)
��$ %
;
��% &
_dispatcher
�� 
=
�� 
null
�� "
;
��" #
}
�� 
}
�� 	
~
�� 	#
StandardLoggerFactory
��	 
(
�� 
)
��  
{
�� 	
Dispose
�� 
(
�� 
false
�� 
)
�� 
;
�� 
}
�� 	
	protected
�� 
virtual
�� 
ITarget
�� !
BuildTarget
��" -
(
��- .
TargetConfig
��. :
targetConfig
��; G
)
��G H
{
�� 	
try
�� 
{
�� 
var
�� 
type
�� 
=
�� 
Type
�� 
.
��  
GetType
��  '
(
��' (
targetConfig
��( 4
.
��4 5
Type
��5 9
)
��9 :
;
��: ;
if
�� 
(
�� 
type
�� 
!=
�� 
null
��  
)
��  !
{
�� 
var
�� 
target
�� 
=
��  
(
��! "
ITarget
��" )
)
��) *
	Activator
��* 3
.
��3 4
CreateInstance
��4 B
(
��B C
type
��C G
)
��G H
;
��H I
target
�� 
.
�� 
	Configure
�� $
(
��$ %
targetConfig
��% 1
)
��1 2
;
��2 3
target
�� 
.
�� 
Name
�� 
=
��  !
targetConfig
��" .
.
��. /
Name
��/ 3
;
��3 4
if
�� 
(
�� 
ILayoutTargetType
�� )
.
��) *
IsAssignableFrom
��* :
(
��: ;
target
��; A
.
��A B
GetType
��B I
(
��I J
)
��J K
)
��K L
)
��L M
{
�� 
var
�� 
layoutTarget
�� (
=
��) *
(
��+ ,
ILayoutTarget
��, 9
)
��9 :
target
��: @
;
��@ A
layoutTarget
�� $
.
��$ %
	Configure
��% .
(
��. /
targetConfig
��/ ;
,
��; <
this
��= A
)
��A B
;
��B C
}
�� 
return
�� 
target
�� !
;
��! "
}
�� 
else
�� 
{
�� 
FallbackLogger
�� "
.
��" #
Log
��# &
(
��& '
$str��' �
,��� �
targetConfig��� �
.��� �
Name��� �
,��� �
targetConfig��� �
.��� �
Type��� �
)��� �
;��� �
return
�� 
null
�� 
;
��  
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
cause
�� "
)
��" #
{
�� 
FallbackLogger
�� 
.
�� 
Log
�� "
(
��" #
$str
��# w
,
��w x
targetConfig
��  
!=
��! #
null
��$ (
?
��) *
targetConfig
��+ 7
.
��7 8
Name
��8 <
:
��= >
string
��? E
.
��E F
Empty
��F K
,
��K L
targetConfig
��  
!=
��! #
null
��$ (
?
��) *
targetConfig
��+ 7
.
��7 8
Type
��8 <
:
��= >
string
��? E
.
��E F
Empty
��F K
,
��K L
cause
�� 
)
�� 
;
�� 
return
�� 
null
�� 
;
�� 
}
�� 
}
�� 	
}
�� 
}�� �
ZC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\FallbackLoggers\StandardFileFallbackLogger.cs
	namespace 	
NuLog
 
. 
FallbackLoggers 
{		 
public 

class &
StandardFileFallbackLogger +
:, -&
StandardFallbackLoggerBase. H
{ 
private 
readonly 
string 
filePath  (
;( )
public &
StandardFileFallbackLogger )
() *
string* 0
filePath1 9
)9 :
{ 	
this 
. 
filePath 
= 
filePath $
;$ %
} 	
public 
override 
void 
Log  
(  !
	Exception! *
	exception+ 4
,4 5
ITarget6 =
target> D
,D E
	ILogEventF O
logEventP X
)X Y
{ 	
var 
	formatted 
= 
FormatMessage )
() *
	exception* 3
,3 4
target5 ;
,; <
logEvent= E
)E F
;F G
File 
. 
AppendAllText 
( 
this #
.# $
filePath$ ,
,, -
	formatted. 7
)7 8
;8 9
} 	
public 
override 
void 
Log  
(  !
string! '
message( /
,/ 0
params1 7
object8 >
[> ?
]? @
argsA E
)E F
{   	
var!! 
	formatted!! 
=!! 
FormatMessage!! )
(!!) *
message!!* 1
,!!1 2
args!!3 7
)!!7 8
;!!8 9
File"" 
."" 
AppendAllText"" 
("" 
this"" #
.""# $
filePath""$ ,
,"", -
	formatted"". 7
)""7 8
;""8 9
}## 	
}$$ 
}%% �'
ZC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\FallbackLoggers\StandardFallbackLoggerBase.cs
	namespace

 	
NuLog


 
.

 
FallbackLoggers

 
{ 
public 

abstract 
class &
StandardFallbackLoggerBase 4
:5 6
IFallbackLogger7 F
{ 
public 
abstract 
void 
Log  
(  !
	Exception! *
	exception+ 4
,4 5
ITarget6 =
target> D
,D E
	ILogEventF O
logEventP X
)X Y
;Y Z
public 
abstract 
void 
Log  
(  !
string! '
message( /
,/ 0
params1 7
object8 >
[> ?
]? @
argsA E
)E F
;F G
	protected 
virtual 
string  
FormatMessage! .
(. /
	Exception/ 8
	exception9 B
,B C
ITargetD K
targetL R
,R S
	ILogEventT ]
logEvent^ f
)f g
{ 	
return 
FormatMessage  
(  !
$str! h
,h i
targetj p
.p q
Nameq u
,u v
targetw }
.} ~
GetType	~ �
(
� �
)
� �
.
� �
FullName
� �
,
� �
JoinTags
� �
(
� �
logEvent
� �
)
� �
,
� �

GetMessage
� �
(
� �
logEvent
� �
)
� �
,
� �!
GetExceptionMessage
� �
(
� �
logEvent
� �
)
� �
,
� �
	exception
� �
)
� �
;
� �
} 	
	protected 
virtual 
string  
FormatMessage! .
(. /
string/ 5
message6 =
,= >
params? E
objectF L
[L M
]M N
argsO S
)S T
{ 	
var 
	formatted 
= 
args  
!=! #
null$ (
&&) +
args, 0
.0 1
Length1 7
>8 9
$num: ;
?< =
string> D
.D E
FormatE K
(K L
messageL S
,S T
argsU Y
)Y Z
:[ \
message] d
;d e
return 
string 
. 
Format  
(  !
$str! D
,D E
DateTimeF N
.N O
NowO R
,R S
	formattedT ]
)] ^
;^ _
} 	
private   
static   
string   

GetMessage   (
(  ( )
	ILogEvent  ) 2
logEvent  3 ;
)  ; <
{!! 	
return"" 
logEvent"" 
is"" 
LogEvent"" '
?""( )
(""* +
(""+ ,
LogEvent"", 4
)""4 5
logEvent""5 =
)""= >
.""> ?
Message""? F
:""G H
logEvent""I Q
.""Q R
ToString""R Z
(""Z [
)""[ \
;""\ ]
}## 	
private%% 
static%% 
string%% 
JoinTags%% &
(%%& '
	ILogEvent%%' 0
logEvent%%1 9
)%%9 :
{&& 	
return** 
logEvent** 
.** 
Tags**  
!=**! #
null**$ (
?**) *
string**+ 1
.**1 2
Join**2 6
(**6 7
$str**7 :
,**: ;
logEvent**< D
.**D E
Tags**E I
)**I J
:**K L
string**M S
.**S T
Empty**T Y
;**Y Z
},, 	
private.. 
static.. 
string.. 
GetExceptionMessage.. 1
(..1 2
	ILogEvent..2 ;
logEvent..< D
)..D E
{// 	
if00 
(00 
logEvent00 
is00 
LogEvent00 $
)00$ %
{11 
var22 
	exception22 
=22 
(22  !
(22! "
LogEvent22" *
)22* +
logEvent22+ 3
)223 4
.224 5
	Exception225 >
;22> ?
return33 
string33 
.33 
Format33 $
(33$ %
$str33% B
,33B C
	exception33D M
!=33N P
null33Q U
?33V W
	exception33X a
.33a b
Message33b i
:33j k
string33l r
.33r s
Empty33s x
)33x y
;33y z
}44 
else55 
{66 
return77 
string77 
.77 
Empty77 #
;77# $
}88 
}99 	
}:: 
};; �
[C:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\FallbackLoggers\StandardTraceFallbackLogger.cs
	namespace 	
NuLog
 
. 
FallbackLoggers 
{		 
public 

class '
StandardTraceFallbackLogger ,
:- .&
StandardFallbackLoggerBase/ I
{ 
public 
override 
void 
Log  
(  !
	Exception! *
	exception+ 4
,4 5
ITarget6 =
target> D
,D E
	ILogEventF O
logEventP X
)X Y
{ 	
var 
message 
= 
this 
. 
FormatMessage ,
(, -
	exception- 6
,6 7
target8 >
,> ?
logEvent@ H
)H I
;I J
Trace 
. 
	WriteLine 
( 
message #
)# $
;$ %
} 	
public 
override 
void 
Log  
(  !
string! '
message( /
,/ 0
params1 7
object8 >
[> ?
]? @
argsA E
)E F
{ 	
var 
	formatted 
= 
this  
.  !
FormatMessage! .
(. /
message/ 6
,6 7
args8 <
)< =
;= >
Trace 
. 
	WriteLine 
( 
	formatted %
)% &
;& '
} 	
} 
} �
9C:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\ILogEvent.cs
	namespace 	
NuLog
 
{		 
public 

	interface 
	ILogEvent 
:  
IDisposable! ,
{ 
ICollection 
< 
string 
> 
Tags  
{! "
get# &
;& '
set( +
;+ ,
}- .
void 
WriteTo 
( 
ITarget 
target #
)# $
;$ %
} 
} �
>C:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\ILoggerFactory.cs
	namespace 	
NuLog
 
{		 
public 

	interface 
ILoggerFactory #
:$ %
IDisposable& 1
{ 
ILogger 
	GetLogger 
( 
IMetaDataProvider +
metaDataProvider, <
,< =
IEnumerable> I
<I J
stringJ P
>P Q
defaultTagsR ]
)] ^
;^ _
} 
} �
7C:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\ILogger.cs
	namespace 	
NuLog
 
{		 
public 

	interface 
ILogger 
{ 
bool 
IncludeStackFrame 
{  
get! $
;$ %
set& )
;) *
}+ ,
void 
Log 
( 
string 
message 
,  
params! '
string( .
[. /
]/ 0
tags1 5
)5 6
;6 7
void 
LogNow 
( 
string 
message "
," #
params$ *
string+ 1
[1 2
]2 3
tags4 8
)8 9
;9 :
void!! 
Log!! 
(!! 
string!! 
message!! 
,!!  

Dictionary!!! +
<!!+ ,
string!!, 2
,!!2 3
object!!4 :
>!!: ;
metaData!!< D
=!!E F
null!!G K
,!!K L
params!!M S
string!!T Z
[!!Z [
]!![ \
tags!!] a
)!!a b
;!!b c
void&& 
LogNow&& 
(&& 
string&& 
message&& "
,&&" #

Dictionary&&$ .
<&&. /
string&&/ 5
,&&5 6
object&&7 =
>&&= >
metaData&&? G
=&&H I
null&&J N
,&&N O
params&&P V
string&&W ]
[&&] ^
]&&^ _
tags&&` d
)&&d e
;&&e f
void++ 
Log++ 
(++ 
	Exception++ 
	exception++ $
,++$ %
string++& ,
message++- 4
,++4 5
params++6 <
string++= C
[++C D
]++D E
tags++F J
)++J K
;++K L
void00 
LogNow00 
(00 
	Exception00 
	exception00 '
,00' (
string00) /
message000 7
,007 8
params009 ?
string00@ F
[00F G
]00G H
tags00I M
)00M N
;00N O
void55 
Log55 
(55 
	Exception55 
	exception55 $
,55$ %
string55& ,
message55- 4
,554 5

Dictionary556 @
<55@ A
string55A G
,55G H
object55I O
>55O P
metaData55Q Y
=55Z [
null55\ `
,55` a
params55b h
string55i o
[55o p
]55p q
tags55r v
)55v w
;55w x
void:: 
LogNow:: 
(:: 
	Exception:: 
	exception:: '
,::' (
string::) /
message::0 7
,::7 8

Dictionary::9 C
<::C D
string::D J
,::J K
object::L R
>::R S
metaData::T \
=::] ^
null::_ c
,::c d
params::e k
string::l r
[::r s
]::s t
tags::u y
)::y z
;::z {
};; 
}<< �
GC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\LogFactoryActivatorNull.cs
	namespace 	
NuLog
 
{ 
public 

sealed 
class #
LogFactoryActivatorNull /
{ 
public 
static 
readonly 
Type #
NullType$ ,
=- .
typeof/ 5
(5 6#
LogFactoryActivatorNull6 M
)M N
;N O
public 
static 
readonly #
LogFactoryActivatorNull 6

NullObject7 A
=B C
newD G#
LogFactoryActivatorNullH _
(_ `
)` a
;a b
private #
LogFactoryActivatorNull '
(' (
)( )
{ 	
} 	
} 
} �
IC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Loggers\IMetaDataProvider.cs
	namespace 	
NuLog
 
{ 
public 

	interface 
IMetaDataProvider &
{ 
IDictionary 
< 
string 
, 
object "
>" #
ProvideMetaData$ 3
(3 4
)4 5
;5 6
} 
} �
FC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Loggers\ITagNormalizer.cs
	namespace 	
NuLog
 
{ 
public 

	interface 
ITagNormalizer #
{ 
string 
NormalizeTag 
( 
string "
tag# &
)& '
;' (
ICollection 
< 
string 
> 
NormalizeTags )
() *
IEnumerable* 5
<5 6
string6 <
>< =
tags> B
)B C
;C D
} 
} �
CC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Dispatchers\ITarget.cs
	namespace		 	
NuLog		
 
{

 
public 

	interface 
ITarget 
: 
IDisposable *
{ 
string 
Name 
{ 
get 
; 
set 
; 
}  !
void 
	Configure 
( 
TargetConfig #
config$ *
)* +
;+ ,
void 
Write 
( 
LogEvent 
logEvent $
)$ %
;% &
} 
} �
GC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Layouts\IPropertyParser.cs
	namespace 	
NuLog
 
. 
Layouts 
{ 
public 

	interface 
IPropertyParser $
{ 
object 
GetProperty 
( 
object !
zobject" )
,) *
string+ 1
[1 2
]2 3
path4 8
)8 9
;9 :
} 
} �
EC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Layouts\ILayoutParser.cs
	namespace 	
NuLog
 
. 
Layouts 
{ 
public 

	interface 
ILayoutParser "
{ 
ICollection 
< 
LayoutParameter #
># $
Parse% *
(* +
string+ 1
format2 8
)8 9
;9 :
} 
} �
GC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Layouts\LayoutParameter.cs
	namespace 	
NuLog
 
. 
Layouts 
{ 
public 

class 
LayoutParameter  
{ 
public 
bool 

StaticText 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
Text 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
Format 
{ 
get "
;" #
set$ '
;' (
}) *
public   
bool   

Contingent   
{    
get  ! $
;  $ %
set  & )
;  ) *
}  + ,
public&& 
string&& 
Path&& 
{&& 
get&&  
;&&  !
set&&" %
;&&% &
}&&' (
}'' 
}(( �C
LC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Layouts\StandardLayoutParser.cs
	namespace		 	
NuLog		
 
.		 
Layouts		 
{

 
public 

class  
StandardLayoutParser %
:& '
ILayoutParser( 5
{ 
private 
static 
readonly 
Regex  %
parameterPattern& 6
=7 8
new9 <
Regex= B
(B C
$strC e
,e f
RegexOptionsg s
.s t
Compiledt |
)| }
;} ~
private 
static 
readonly 
Regex  % 
parameterNamePattern& :
=; <
new= @
RegexA F
(F G
$strG U
,U V
RegexOptionsW c
.c d
Compiledd l
)l m
;m n
private 
static 
readonly 
Regex  %"
parameterFormatPattern& <
== >
new? B
RegexC H
(H I
$strI Y
,Y Z
RegexOptions[ g
.g h
Compiledh p
)p q
;q r
public 
ICollection 
< 
LayoutParameter *
>* +
Parse, 1
(1 2
string2 8
format9 ?
)? @
{ 	
var 

parameters 
= 
new  
List! %
<% &
LayoutParameter& 5
>5 6
(6 7
)7 8
;8 9
var 
parameterTexts 
=  
FindParameterTexts! 3
(3 4
format4 :
): ;
;; <
string 
runningFormat  
=! "
format# )
;) *
int 
parameterIndex 
; 
string 

staticText 
; 
foreach!! 
(!! 
var!! 
parameterText!! &
in!!' )
parameterTexts!!* 8
)!!8 9
{"" 
parameterIndex## 
=##  
runningFormat##! .
.##. /
IndexOf##/ 6
(##6 7
parameterText##7 D
,##D E
StringComparison##F V
.##V W
Ordinal##W ^
)##^ _
;##_ `
if$$ 
($$ 
parameterIndex$$ "
>$$# $
$num$$% &
)$$& '
{%% 

staticText&& 
=&&  
runningFormat&&! .
.&&. /
	Substring&&/ 8
(&&8 9
$num&&9 :
,&&: ;
parameterIndex&&< J
)&&J K
;&&K L

parameters'' 
.'' 
Add'' "
(''" #
new''# &
LayoutParameter''' 6
{(( 

StaticText)) "
=))# $
true))% )
,))) *
Text** 
=** 

staticText** )
}++ 
)++ 
;++ 
},, 
runningFormat-- 
=-- 
runningFormat--  -
.--- .
	Substring--. 7
(--7 8
parameterIndex--8 F
+--G H
parameterText--I V
.--V W
Length--W ]
)--] ^
;--^ _

parameters// 
.// 
Add// 
(// 
ParseParameter// -
(//- .
parameterText//. ;
)//; <
)//< =
;//= >
}00 
if33 
(33 
string33 
.33 
IsNullOrEmpty33 $
(33$ %
runningFormat33% 2
)332 3
==334 6
false337 <
)33< =
{44 

parameters55 
.55 
Add55 
(55 
new55 "
LayoutParameter55# 2
{66 

StaticText77 
=77  
true77! %
,77% &
Text88 
=88 
runningFormat88 (
}99 
)99 
;99 
}:: 
return== 

parameters== 
;== 
}>> 	
privateCC 
staticCC 
LayoutParameterCC &
ParseParameterCC' 5
(CC5 6
stringCC6 <
textCC= A
)CCA B
{DD 	
varFF 
parmFF 
=FF 
newFF 
LayoutParameterFF *
{GG 
TextHH 
=HH 
textHH 
,HH 
PathII 
=II  
parameterNamePatternII +
.II+ ,
MatchII, 1
(II1 2
textII2 6
)II6 7
.II7 8
ToStringII8 @
(II@ A
)IIA B
}JJ 
;JJ 
parmNN 
.NN 

ContingentNN 
=NN 
parmNN "
.NN" #
PathNN# '
[NN' (
$numNN( )
]NN) *
==NN+ -
$charNN. 1
;NN1 2
parmOO 
.OO 
PathOO 
=OO 
parmOO 
.OO 

ContingentOO '
?PP 
parmPP 
.PP 
PathPP 
.PP 
	SubstringPP %
(PP% &
$numPP& '
)PP' (
:QQ 
parmQQ 
.QQ 
PathQQ 
.QQ 
	SubstringQQ %
(QQ% &
$numQQ& '
)QQ' (
;QQ( )
parmTT 
.TT 
FormatTT 
=TT "
parameterFormatPatternTT 0
.TT0 1
MatchTT1 6
(TT6 7
textTT7 ;
)TT; <
.TT< =
ToStringTT= E
(TTE F
)TTF G
;TTG H
parmUU 
.UU 
FormatUU 
=UU 
!UU 
stringUU !
.UU! "
IsNullOrEmptyUU" /
(UU/ 0
parmUU0 4
.UU4 5
FormatUU5 ;
)UU; <
?VV 
parmVV 
.VV 
FormatVV 
.VV 
	SubstringVV '
(VV' (
$numVV( )
)VV) *
:WW 
nullWW 
;WW 
parmYY 
.YY 
FormatYY 
=YY 
!YY 
stringYY !
.YY! "
IsNullOrEmptyYY" /
(YY/ 0
parmYY0 4
.YY4 5
FormatYY5 ;
)YY; <
&&YY= ?
parmYY@ D
.YYD E
FormatYYE K
.YYK L

StartsWithYYL V
(YYV W
$strYYW Z
)YYZ [
&&YY\ ^
parmYY_ c
.YYc d
FormatYYd j
.YYj k
EndsWithYYk s
(YYs t
$strYYt w
)YYw x
?ZZ 
parmZZ 
.ZZ 
FormatZZ 
.ZZ 
	SubstringZZ '
(ZZ' (
$numZZ( )
,ZZ) *
parmZZ+ /
.ZZ/ 0
FormatZZ0 6
.ZZ6 7
LengthZZ7 =
-ZZ> ?
$numZZ@ A
)ZZA B
:[[ 
parm[[ 
.[[ 
Format[[ 
;[[ 
parm]] 
.]] 
Format]] 
=]] 
!]] 
string]] !
.]]! "
IsNullOrEmpty]]" /
(]]/ 0
parm]]0 4
.]]4 5
Format]]5 ;
)]]; <
?^^ 
parm^^ 
.^^ 
Format^^ 
.^^ 
Replace^^ %
(^^% &
$str^^& +
,^^+ ,
$str^^- 0
)^^0 1
.^^1 2
Replace^^2 9
(^^9 :
$str^^: @
,^^@ A
$str^^B F
)^^F G
:__ 
parm__ 
.__ 
Format__ 
;__ 
returnaa 
parmaa 
;aa 
}bb 	
privateii 
staticii 
ICollectionii "
<ii" #
stringii# )
>ii) *
FindParameterTextsii+ =
(ii= >
stringii> D
formatiiE K
)iiK L
{jj 	
varkk 
listkk 
=kk 
newkk 
Listkk 
<kk  
stringkk  &
>kk& '
(kk' (
)kk( )
;kk) *
varll 
matchesll 
=ll 
parameterPatternll *
.ll* +
Matchesll+ 2
(ll2 3
formatll3 9
)ll9 :
;ll: ;
foreachmm 
(mm 
varmm 
matchmm 
inmm !
matchesmm" )
)mm) *
listnn 
.nn 
Addnn 
(nn 
matchnn 
.nn 
ToStringnn '
(nn' (
)nn( )
)nn) *
;nn* +
returnoo 
listoo 
;oo 
}pp 	
}qq 
}rr �=
NC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Layouts\StandardPropertyParser.cs
	namespace		 	
NuLog		
 
.		 
Layouts		 
{

 
public 

class "
StandardPropertyParser '
:( )
IPropertyParser* 9
{ 
private 
static 
readonly 
Type  $
DictionaryType% 3
=4 5
typeof6 <
(< =
IDictionary= H
<H I
stringI O
,O P
objectQ W
>W X
)X Y
;Y Z
private 
readonly 
IDictionary $
<$ %
Type% )
,) *
IDictionary+ 6
<6 7
string7 =
,= >
PropertyInfo? K
>K L
>L M
	typeCacheN W
;W X
private 
readonly 
IDictionary $
<$ %
Type% )
,) *
bool+ /
>/ 0
dictionaryTypes1 @
;@ A
public "
StandardPropertyParser %
(% &
)& '
{ 	
this 
. 
	typeCache 
= 
new  

Dictionary! +
<+ ,
Type, 0
,0 1
IDictionary2 =
<= >
string> D
,D E
PropertyInfoF R
>R S
>S T
(T U
)U V
;V W
this 
. 
dictionaryTypes  
=! "
new# &

Dictionary' 1
<1 2
Type2 6
,6 7
bool8 <
>< =
(= >
)> ?
;? @
} 	
public 
object 
GetProperty !
(! "
object" (
zobject) 0
,0 1
string2 8
[8 9
]9 :
path; ?
)? @
{ 	
if   
(   
path   
==   
null   
)   
{!! 
return"" 
null"" 
;"" 
}## 
return&& 
GetPropertyRecurse&& %
(&&% &
zobject&&& -
,&&- .
path&&/ 3
)&&3 4
;&&4 5
}'' 	
private00 
object00 
GetPropertyRecurse00 )
(00) *
object00* 0
zobject001 8
,008 9
string00: @
[00@ A
]00A B
propertyChain00C P
,00P Q
int00R U
depth00V [
=00\ ]
$num00^ _
)00_ `
{11 	
if33 
(33 
zobject33 
==33 
null33 
||33  "
depth33# (
>=33) +
propertyChain33, 9
.339 :
Length33: @
)33@ A
{44 
return77 
zobject77 
;77 
}88 
else99 
{:: 
var;; 
zobjectType;; 
=;;  !
zobject;;" )
.;;) *
GetType;;* 1
(;;1 2
);;2 3
;;;3 4
var<< 
	chainItem<< 
=<< 
propertyChain<<  -
[<<- .
depth<<. 3
]<<3 4
;<<4 5
if?? 
(?? 
!?? 
IsDictionaryType?? %
(??% &
zobjectType??& 1
)??1 2
)??2 3
{@@ 
varBB 
propertyDictBB $
=BB% &
GetPropertyInfoBB' 6
(BB6 7
zobjectTypeBB7 B
)BBB C
;BBC D
varCC 
propertyInfoCC $
=CC% &
propertyDictCC' 3
.CC3 4
ContainsKeyCC4 ?
(CC? @
	chainItemCC@ I
)CCI J
?DD 
propertyDictDD &
[DD& '
	chainItemDD' 0
]DD0 1
:EE 
nullEE 
;EE 
ifFF 
(FF 
propertyInfoFF $
!=FF% '
nullFF( ,
)FF, -
{GG 
returnHH 
GetPropertyRecurseHH 1
(HH1 2
propertyInfoHH2 >
.HH> ?
GetValueHH? G
(HHG H
zobjectHHH O
,HHO P
nullHHQ U
)HHU V
,HHV W
propertyChainHHX e
,HHe f
depthHHg l
+HHm n
$numHHo p
)HHp q
;HHq r
}II 
elseJJ 
{KK 
returnLL 
nullLL #
;LL# $
}MM 
}NN 
elseOO 
{PP 
varRR 

dictionaryRR "
=RR# $
(RR% &
IDictionaryRR& 1
<RR1 2
stringRR2 8
,RR8 9
objectRR: @
>RR@ A
)RRA B
zobjectRRB I
;RRI J
ifSS 
(SS 

dictionarySS "
.SS" #
ContainsKeySS# .
(SS. /
	chainItemSS/ 8
)SS8 9
)SS9 :
{TT 
returnUU 
GetPropertyRecurseUU 1
(UU1 2

dictionaryUU2 <
[UU< =
	chainItemUU= F
]UUF G
,UUG H
propertyChainUUI V
,UUV W
depthUUX ]
+UU^ _
$numUU` a
)UUa b
;UUb c
}VV 
elseWW 
{XX 
returnYY 
nullYY #
;YY# $
}ZZ 
}[[ 
}\\ 
}]] 	
privatecc 
IDictionarycc 
<cc 
stringcc "
,cc" #
PropertyInfocc$ 0
>cc0 1
GetPropertyInfocc2 A
(ccA B
TypeccB F

objectTypeccG Q
)ccQ R
{dd 	
ifhh 
(hh 
!hh 
	typeCachehh 
.hh 
ContainsKeyhh &
(hh& '

objectTypehh' 1
)hh1 2
)hh2 3
{ii 
varjj 
propertyInfojj  
=jj! "

objectTypejj# -
.jj- .
GetPropertiesjj. ;
(jj; <
)jj< =
;jj= >
varkk 
dictkk 
=kk 
newkk 

Dictionarykk )
<kk) *
stringkk* 0
,kk0 1
PropertyInfokk2 >
>kk> ?
(kk? @
)kk@ A
;kkA B
foreachll 
(ll 
varll 
propertyll %
inll& (
propertyInfoll) 5
)ll5 6
{mm 
dictnn 
[nn 
propertynn !
.nn! "
Namenn" &
]nn& '
=nn( )
propertynn* 2
;nn2 3
}oo 
	typeCachepp 
[pp 

objectTypepp $
]pp$ %
=pp& '
dictpp( ,
;pp, -
returnqq 
dictqq 
;qq 
}rr 
returnuu 
	typeCacheuu 
[uu 

objectTypeuu '
]uu' (
;uu( )
}vv 	
private~~ 
bool~~ 
IsDictionaryType~~ %
(~~% &
Type~~& *

objectType~~+ 5
)~~5 6
{ 	
if
�� 
(
�� 
dictionaryTypes
�� 
.
��  
ContainsKey
��  +
(
��+ ,

objectType
��, 6
)
��6 7
)
��7 8
{
�� 
return
�� 
dictionaryTypes
�� &
[
��& '

objectType
��' 1
]
��1 2
;
��2 3
}
�� 
var
�� 
isAssignable
�� 
=
�� 
DictionaryType
�� -
.
��- .
IsAssignableFrom
��. >
(
��> ?

objectType
��? I
)
��I J
;
��J K
dictionaryTypes
�� 
[
�� 

objectType
�� &
]
��& '
=
��( )
isAssignable
��* 6
;
��6 7
return
�� 
isAssignable
�� 
;
��  
}
�� 	
}
�� 
}�� �h
FC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Layouts\StandardLayout.cs
	namespace 	
NuLog
 
. 
Layouts 
{ 
public 

class 
StandardLayout 
:  !
ILayout" )
{ 
private 
static 
readonly 
Type  $
iEnumerableType% 4
=5 6
typeof7 =
(= >
IEnumerable> I
)I J
;J K
private!! 
static!! 
readonly!! 
Type!!  $

stringType!!% /
=!!0 1
typeof!!2 8
(!!8 9
string!!9 ?
)!!? @
;!!@ A
private&& 
readonly&& 
IEnumerable&& $
<&&$ %
LayoutParameter&&% 4
>&&4 5
layoutParameters&&6 F
;&&F G
private++ 
readonly++ 
IPropertyParser++ (
propertyParser++) 7
;++7 8
private00 
readonly00 
IDictionary00 $
<00$ %
string00% +
,00+ ,
string00- 3
[003 4
]004 5
>005 6
splitPathCache007 E
;00E F
public55 
StandardLayout55 
(55 
IEnumerable55 )
<55) *
LayoutParameter55* 9
>559 :
layoutParameters55; K
,55K L
IPropertyParser55M \
propertyParser55] k
)55k l
{66 	
this77 
.77 
layoutParameters77 !
=77" #
layoutParameters77$ 4
;774 5
this99 
.99 
propertyParser99 
=99  !
propertyParser99" 0
;990 1
this;; 
.;; 
splitPathCache;; 
=;;  !
new;;" %

Dictionary;;& 0
<;;0 1
string;;1 7
,;;7 8
string;;9 ?
[;;? @
];;@ A
>;;A B
(;;B C
);;C D
;;;D E
}<< 	
public>> 
string>> 
Format>> 
(>> 
LogEvent>> %
logEvent>>& .
)>>. /
{?? 	
var@@ 
messageBuilder@@ 
=@@  
new@@! $
StringBuilder@@% 2
(@@2 3
)@@3 4
;@@4 5
foreachBB 
(BB 
varBB 
	parameterBB "
inBB# %
thisBB& *
.BB* +
layoutParametersBB+ ;
)BB; <
{CC 
messageBuilderDD 
.DD 
AppendDD %
(DD% &
FormatParameterDD& 5
(DD5 6
logEventDD6 >
,DD> ?
	parameterDD@ I
)DDI J
)DDJ K
;DDK L
}EE 
returnGG 
messageBuilderGG !
.GG! "
ToStringGG" *
(GG* +
)GG+ ,
;GG, -
}HH 	
privateMM 
stringMM 
FormatParameterMM &
(MM& '
LogEventMM' /
logEventMM0 8
,MM8 9
LayoutParameterMM: I
	parameterMMJ S
)MMS T
{NN 	
ifOO 
(OO 
	parameterOO 
.OO 

StaticTextOO $
)OO$ %
{PP 
returnQQ 
	parameterQQ  
.QQ  !
TextQQ! %
;QQ% &
}RR 
elseSS 
{TT 
varXX 
parameterValueXX "
=XX# $
GetSpecialParameterXX% 8
(XX8 9
logEventXX9 A
,XXA B
	parameterXXC L
)XXL M
;XXM N
if[[ 
([[ 
parameterValue[[ "
==[[# %
null[[& *
)[[* +
{\\ 
var^^ 
path^^ 
=^^ 
GetSplitPath^^ +
(^^+ ,
	parameter^^, 5
.^^5 6
Path^^6 :
)^^: ;
;^^; <
parameterValueaa "
=aa# $
parameterValueaa% 3
??aa4 6
thisaa7 ;
.aa; <
propertyParseraa< J
.aaJ K
GetPropertyaaK V
(aaV W
logEventaaW _
.aa_ `
MetaDataaa` h
,aah i
pathaaj n
)aan o
;aao p
parameterValuedd "
=dd# $
parameterValuedd% 3
??dd4 6
thisdd7 ;
.dd; <
propertyParserdd< J
.ddJ K
GetPropertyddK V
(ddV W
logEventddW _
,dd_ `
pathdda e
)dde f
;ddf g
}ee 
varhh 
parameterStringhh #
=hh$ %
!hh& '
	parameterhh' 0
.hh0 1

Contingenthh1 ;
||hh< >
!hh? @
IsNullOrEmptyStringhh@ S
(hhS T
parameterValuehhT b
)hhb c
?ii 
GetFormattedValueii '
(ii' (
parameterValueii( 6
,ii6 7
	parameterii8 A
.iiA B
FormatiiB H
)iiH I
:jj 
stringjj 
.jj 
Emptyjj "
;jj" #
returnll 
parameterStringll &
;ll& '
}mm 
}nn 	
privatess 
stringss 
[ss 
]ss 
GetSplitPathss %
(ss% &
stringss& ,
pathss- 1
)ss1 2
{tt 	
ifuu 
(uu 
splitPathCacheuu 
.uu 
ContainsKeyuu *
(uu* +
pathuu+ /
)uu/ 0
)uu0 1
{vv 
returnww 
splitPathCacheww %
[ww% &
pathww& *
]ww* +
;ww+ ,
}xx 
varzz 
	splitPathzz 
=zz 
pathzz  
.zz  !
Splitzz! &
(zz& '
$charzz' *
)zz* +
;zz+ ,
splitPathCache{{ 
[{{ 
path{{ 
]{{  
={{! "
	splitPath{{# ,
;{{, -
return|| 
	splitPath|| 
;|| 
}}} 	
	protected
�� 
virtual
�� 
object
��  !
GetSpecialParameter
��! 4
(
��4 5
LogEvent
��5 =
logEvent
��> F
,
��F G
LayoutParameter
��H W
	parameter
��X a
)
��a b
{
�� 	
switch
�� 
(
�� 
	parameter
�� 
.
�� 
Path
�� "
)
��" #
{
�� 
case
�� 
$str
�� 
:
�� 
return
�� 
logEvent
�� #
.
��# $
Tags
��$ (
!=
��) +
null
��, 0
?
��1 2
string
��3 9
.
��9 :
Join
��: >
(
��> ?
$str
��? B
,
��B C
logEvent
��D L
.
��L M
Tags
��M Q
)
��Q R
:
��S T
string
��U [
.
��[ \
Empty
��\ a
;
��a b
case
�� 
$str
��  
:
��  !
return
�� 
FormatException
�� *
(
��* +
logEvent
��+ 3
.
��3 4
	Exception
��4 =
)
��= >
;
��> ?
default
�� 
:
�� 
return
�� 
null
�� 
;
��  
}
�� 
}
�� 	
private
�� 
static
�� 
string
�� 
FormatException
�� -
(
��- .
	Exception
��. 7
	exception
��8 A
)
��A B
{
�� 	
var
�� 
sb
�� 
=
�� 
new
�� 
StringBuilder
�� &
(
��& '
)
��' (
;
��( )
var
�� 
depth
�� 
=
�� 
$num
�� 
;
�� 
bool
�� 
inner
�� 
=
�� 
false
�� 
;
�� 
while
�� 
(
�� 
	exception
�� 
!=
�� 
null
��  $
&&
��% '
depth
��( -
++
��- /
<
��0 1
$num
��2 4
)
��4 5
{
�� 
sb
�� 
.
�� 
Append
�� 
(
�� 
string
��  
.
��  !
Format
��! '
(
��' (
$str
��( 9
,
��9 :
inner
��; @
?
��A B
$str
��C O
:
��P Q
$str
��R T
,
��T U
	exception
��V _
.
��_ `
GetType
��` g
(
��g h
)
��h i
.
��i j
FullName
��j r
,
��r s
	exception
��t }
.
��} ~
Message��~ �
)��� �
)��� �
;��� �
sb
�� 
.
�� 
Append
�� 
(
�� 
string
��  
.
��  !
Format
��! '
(
��' (
$str
��( 1
,
��1 2
	exception
��3 <
.
��< =

StackTrace
��= G
)
��G H
)
��H I
;
��I J
	exception
�� 
=
�� 
	exception
�� %
.
��% &
InnerException
��& 4
;
��4 5
inner
�� 
=
�� 
true
�� 
;
�� 
}
�� 
return
�� 
sb
�� 
.
�� 
ToString
�� 
(
�� 
)
��  
;
��  !
}
�� 	
private
�� 
static
�� 
bool
�� !
IsNullOrEmptyString
�� /
(
��/ 0
object
��0 6
value
��7 <
)
��< =
{
�� 	
return
�� 
value
�� 
==
�� 
null
��  
||
�� 
(
�� 
typeof
�� 
(
�� 
string
�� !
)
��! "
.
��" #
IsAssignableFrom
��# 3
(
��3 4
value
��4 9
.
��9 :
GetType
��: A
(
��A B
)
��B C
)
��C D
&&
��E G
string
��H N
.
��N O
IsNullOrEmpty
��O \
(
��\ ]
(
��] ^
string
��^ d
)
��d e
value
��e j
)
��j k
)
��k l
;
��l m
}
�� 	
private
�� 
static
�� 
string
�� 
GetFormattedValue
�� /
(
��/ 0
object
��0 6
value
��7 <
,
��< =
string
��> D
format
��E K
)
��K L
{
�� 	
if
�� 
(
�� 
value
�� 
==
�� 
null
�� 
)
�� 
{
�� 
return
�� 
string
�� 
.
�� 
Empty
�� #
;
��# $
}
�� 
if
�� 
(
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� $
(
��$ %
format
��% +
)
��+ ,
==
��- /
false
��0 5
)
��5 6
{
�� 
return
�� 
string
�� 
.
�� 
Format
�� $
(
��$ %
format
��% +
,
��+ ,
value
��- 2
)
��2 3
;
��3 4
}
�� 
if
�� 
(
�� 
value
�� 
is
�� 
string
�� 
)
��  
{
�� 
return
�� 
(
�� 
string
�� 
)
�� 
value
�� $
;
��$ %
}
�� 
else
�� 
if
�� 
(
�� 
iEnumerableType
�� $
.
��$ %
IsAssignableFrom
��% 5
(
��5 6
value
��6 ;
.
��; <
GetType
��< C
(
��C D
)
��D E
)
��E F
==
��G I
false
��J O
)
��O P
{
�� 
return
�� 
Convert
�� 
.
�� 
ToString
�� '
(
��' (
value
��( -
)
��- .
;
��. /
}
�� 
else
�� 
{
�� 
return
�� 
string
�� 
.
�� 
Format
�� $
(
��$ %
$str
��% ,
,
��, -
string
��. 4
.
��4 5
Join
��5 9
(
��9 :
$str
��: =
,
��= >
EnumerateValues
��? N
(
��N O
(
��O P
IEnumerable
��P [
)
��[ \
value
��\ a
)
��a b
)
��b c
)
��c d
;
��d e
}
�� 
}
�� 	
private
�� 
static
�� 
string
�� 
[
�� 
]
�� 
EnumerateValues
��  /
(
��/ 0
IEnumerable
��0 ;
items
��< A
)
��A B
{
�� 	
var
�� 
list
�� 
=
�� 
new
�� 
List
�� 
<
��  
string
��  &
>
��& '
(
��' (
)
��( )
;
��) *
foreach
�� 
(
�� 
var
�� 
item
�� 
in
��  
items
��! &
)
��& '
{
�� 
list
�� 
.
�� 
Add
�� 
(
�� 
Convert
��  
.
��  !
ToString
��! )
(
��) *
item
��* .
)
��. /
)
��/ 0
;
��0 1
}
�� 
return
�� 
list
�� 
.
�� 
ToArray
�� 
(
��  
)
��  !
;
��! "
}
�� 	
}
�� 
}�� �
BC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\LogEvents\LogEvent.cs
	namespace

 	
NuLog


 
.

 
	LogEvents

 
{ 
public 

class 
LogEvent 
: 
	ILogEvent %
{ 
public 
DateTime 

DateLogged "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
Thread 
Thread 
{ 
get "
;" #
set$ '
;' (
}) *
public"" 

StackFrame"" 
LoggingStackFrame"" +
{"", -
get"". 1
;""1 2
set""3 6
;""6 7
}""8 9
public'' 
string'' 
Message'' 
{'' 
get''  #
;''# $
set''% (
;''( )
}''* +
public,, 
ICollection,, 
<,, 
string,, !
>,,! "
Tags,,# '
{,,( )
get,,* -
;,,- .
set,,/ 2
;,,2 3
},,4 5
public11 
	Exception11 
	Exception11 "
{11# $
get11% (
;11( )
set11* -
;11- .
}11/ 0
public88 
IDictionary88 
<88 
string88 !
,88! "
object88# )
>88) *
MetaData88+ 3
{884 5
get886 9
;889 :
set88; >
;88> ?
}88@ A
public>> 
virtual>> 
void>> 
WriteTo>> #
(>># $
ITarget>>$ +
target>>, 2
)>>2 3
{?? 	
target@@ 
.@@ 
Write@@ 
(@@ 
this@@ 
)@@ 
;@@ 
}AA 	
publicCC 
virtualCC 
voidCC 
DisposeCC #
(CC# $
)CC$ %
{DD 	
}FF 	
}GG 
}HH �
YC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Configuration\ConfigurationSectionHandler.cs
	namespace 	
NuLog
 
. 
Configuration 
{		 
public

 

class

 '
ConfigurationSectionHandler

 ,
:

- .(
IConfigurationSectionHandler

/ K
{ 
public 
object 
Create 
( 
object #
parent$ *
,* +
object, 2
configContext3 @
,@ A
XmlNodeB I
sectionJ Q
)Q R
{ 	
return 
section 
; 
} 	
} 
}   �
CC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Loggers\IDispatcher.cs
	namespace 	
NuLog
 
. 
Loggers 
{ 
public 

	interface 
IDispatcher  
:! "
IDisposable# .
{ 
void 
DispatchNow 
( 
	ILogEvent "
logEvent# +
)+ ,
;, -
void 
EnqueueForDispatch 
(  
	ILogEvent  )
logEvent* 2
)2 3
;3 4
} 
} �
FC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Loggers\StandardLogger.cs
	namespace 	
NuLog
 
. 
Loggers 
{ 
public 

class 
StandardLogger 
:  !
ILogger" )
{ 
private 
const 
string 
exceptionTag )
=* +
$str, 7
;7 8
	protected 
readonly 
IDispatcher &

Dispatcher' 1
;1 2
	protected   
readonly   
ITagNormalizer   )
TagNormalizer  * 7
;  7 8
	protected%% 
readonly%% 
IMetaDataProvider%% ,
MetaDataProvider%%- =
;%%= >
	protected** 
readonly** 
ICollection** &
<**& '
string**' -
>**- .
DefaultTags**/ :
;**: ;
	protected// 
readonly// 
IDictionary// &
<//& '
string//' -
,//- .
object/// 5
>//5 6
DefaultMetaData//7 F
;//F G
public44 
bool44 
IncludeStackFrame44 %
{44& '
get44( +
;44+ ,
set44- 0
;440 1
}442 3
public99 
StandardLogger99 
(99 
IDispatcher99 )

dispatcher99* 4
,994 5
ITagNormalizer996 D
tagNormalizer99E R
,99R S
IMetaDataProvider99T e
metaDataProvider99f v
,99v w
IEnumerable	99x �
<
99� �
string
99� �
>
99� �
defaultTags
99� �
=
99� �
null
99� �
,
99� �
IDictionary
99� �
<
99� �
string
99� �
,
99� �
object
99� �
>
99� �
defaultMetaData
99� �
=
99� �
null
99� �
,
99� �
bool
99� �
includeStackFrame
99� �
=
99� �
false
99� �
)
99� �
{:: 	

Dispatcher;; 
=;; 

dispatcher;; #
;;;# $
MetaDataProvider== 
=== 
metaDataProvider== /
;==/ 0
TagNormalizer?? 
=?? 
tagNormalizer?? )
;??) *
DefaultTagsAA 
=AA 
tagNormalizerAA '
.AA' (
NormalizeTagsAA( 5
(AA5 6
defaultTagsAA6 A
)AAA B
;AAB C
DefaultMetaDataCC 
=CC 
defaultMetaDataCC -
;CC- .
IncludeStackFrameEE 
=EE 
includeStackFrameEE  1
;EE1 2
}FF 	
publicHH 
voidHH 
LogHH 
(HH 
stringHH 
messageHH &
,HH& '
paramsHH( .
stringHH/ 5
[HH5 6
]HH6 7
tagsHH8 <
)HH< =
{II 	

DispatcherJJ 
.JJ 
EnqueueForDispatchJJ )
(JJ) *
BuildLogEventJJ* 7
(JJ7 8
messageJJ8 ?
,JJ? @
nullJJA E
,JJE F
nullJJG K
,JJK L
tagsJJM Q
)JJQ R
)JJR S
;JJS T
}KK 	
publicMM 
voidMM 
LogNowMM 
(MM 
stringMM !
messageMM" )
,MM) *
paramsMM+ 1
stringMM2 8
[MM8 9
]MM9 :
tagsMM; ?
)MM? @
{NN 	

DispatcherOO 
.OO 
DispatchNowOO "
(OO" #
BuildLogEventOO# 0
(OO0 1
messageOO1 8
,OO8 9
nullOO: >
,OO> ?
nullOO@ D
,OOD E
tagsOOF J
)OOJ K
)OOK L
;OOL M
}PP 	
publicRR 
voidRR 
LogRR 
(RR 
stringRR 
messageRR &
,RR& '

DictionaryRR( 2
<RR2 3
stringRR3 9
,RR9 :
objectRR; A
>RRA B
metaDataRRC K
=RRL M
nullRRN R
,RRR S
paramsRRT Z
stringRR[ a
[RRa b
]RRb c
tagsRRd h
)RRh i
{SS 	

DispatcherTT 
.TT 
EnqueueForDispatchTT )
(TT) *
BuildLogEventTT* 7
(TT7 8
messageTT8 ?
,TT? @
nullTTA E
,TTE F
metaDataTTG O
,TTO P
tagsTTQ U
)TTU V
)TTV W
;TTW X
}UU 	
publicWW 
voidWW 
LogNowWW 
(WW 
stringWW !
messageWW" )
,WW) *

DictionaryWW+ 5
<WW5 6
stringWW6 <
,WW< =
objectWW> D
>WWD E
metaDataWWF N
=WWO P
nullWWQ U
,WWU V
paramsWWW ]
stringWW^ d
[WWd e
]WWe f
tagsWWg k
)WWk l
{XX 	

DispatcherYY 
.YY 
DispatchNowYY "
(YY" #
BuildLogEventYY# 0
(YY0 1
messageYY1 8
,YY8 9
nullYY: >
,YY> ?
metaDataYY@ H
,YYH I
tagsYYJ N
)YYN O
)YYO P
;YYP Q
}ZZ 	
public\\ 
void\\ 
Log\\ 
(\\ 
	Exception\\ !
	exception\\" +
,\\+ ,
string\\- 3
message\\4 ;
,\\; <
params\\= C
string\\D J
[\\J K
]\\K L
tags\\M Q
)\\Q R
{]] 	

Dispatcher^^ 
.^^ 
EnqueueForDispatch^^ )
(^^) *
BuildLogEvent^^* 7
(^^7 8
message^^8 ?
,^^? @
	exception^^A J
,^^J K
null^^L P
,^^P Q
tags^^R V
)^^V W
)^^W X
;^^X Y
}__ 	
publicaa 
voidaa 
LogNowaa 
(aa 
	Exceptionaa $
	exceptionaa% .
,aa. /
stringaa0 6
messageaa7 >
,aa> ?
paramsaa@ F
stringaaG M
[aaM N
]aaN O
tagsaaP T
)aaT U
{bb 	

Dispatchercc 
.cc 
DispatchNowcc "
(cc" #
BuildLogEventcc# 0
(cc0 1
messagecc1 8
,cc8 9
	exceptioncc: C
,ccC D
nullccE I
,ccI J
tagsccK O
)ccO P
)ccP Q
;ccQ R
}dd 	
publicff 
voidff 
Logff 
(ff 
	Exceptionff !
	exceptionff" +
,ff+ ,
stringff- 3
messageff4 ;
,ff; <

Dictionaryff= G
<ffG H
stringffH N
,ffN O
objectffP V
>ffV W
metaDataffX `
=ffa b
nullffc g
,ffg h
paramsffi o
stringffp v
[ffv w
]ffw x
tagsffy }
)ff} ~
{gg 	

Dispatcherhh 
.hh 
EnqueueForDispatchhh )
(hh) *
BuildLogEventhh* 7
(hh7 8
messagehh8 ?
,hh? @
	exceptionhhA J
,hhJ K
metaDatahhL T
,hhT U
tagshhV Z
)hhZ [
)hh[ \
;hh\ ]
}ii 	
publickk 
voidkk 
LogNowkk 
(kk 
	Exceptionkk $
	exceptionkk% .
,kk. /
stringkk0 6
messagekk7 >
,kk> ?

Dictionarykk@ J
<kkJ K
stringkkK Q
,kkQ R
objectkkS Y
>kkY Z
metaDatakk[ c
=kkd e
nullkkf j
,kkj k
paramskkl r
stringkks y
[kky z
]kkz {
tags	kk| �
)
kk� �
{ll 	

Dispatchermm 
.mm 
DispatchNowmm "
(mm" #
BuildLogEventmm# 0
(mm0 1
messagemm1 8
,mm8 9
	exceptionmm: C
,mmC D
metaDatammE M
,mmM N
tagsmmO S
)mmS T
)mmT U
;mmU V
}nn 	
	protectedss 
virtualss 
LogEventss "
BuildLogEventss# 0
(ss0 1
stringss1 7
messagess8 ?
,ss? @
	ExceptionssA J
	exceptionssK T
,ssT U

DictionaryssV `
<ss` a
stringssa g
,ssg h
objectssi o
>sso p
metaDatassq y
,ssy z
string	ss{ �
[
ss� �
]
ss� �
tags
ss� �
)
ss� �
{tt 	
returnuu 
newuu 
LogEventuu 
{vv 
Messageww 
=ww 
messageww !
,ww! "
	Exceptionxx 
=xx 
	exceptionxx %
,xx% &
Tagsyy 
=yy 
GetTagsyy 
(yy 
tagsyy #
,yy# $
	exceptionyy% .
!=yy/ 1
nullyy2 6
)yy6 7
,yy7 8
MetaDatazz 
=zz 
GetMetaDatazz &
(zz& '
metaDatazz' /
)zz/ 0
,zz0 1

DateLogged{{ 
={{ 
DateTime{{ %
.{{% &
UtcNow{{& ,
,{{, -
Thread|| 
=|| 
Thread|| 
.||  
CurrentThread||  -
,||- .
LoggingStackFrame}} !
=}}" #
this}}$ (
.}}( )
IncludeStackFrame}}) :
?}}; <
new}}= @

StackFrame}}A K
(}}K L
$num}}L M
,}}M N
false}}O T
)}}T U
:}}V W
null}}X \
}~~ 
;~~ 
} 	
	protected
�� 
virtual
�� 
ICollection
�� %
<
��% &
string
��& ,
>
��, -
GetTags
��. 5
(
��5 6
ICollection
��6 A
<
��A B
string
��B H
>
��H I
	givenTags
��J S
,
��S T
bool
��U Y
hasException
��Z f
)
��f g
{
�� 	
ICollection
�� 
<
�� 
string
�� 
>
�� 
tags
��  $
;
��$ %
if
�� 
(
�� 
!
�� 
HasTags
�� 
(
�� 
	givenTags
�� "
)
��" #
&&
��$ &
!
��' (
HasTags
��( /
(
��/ 0
DefaultTags
��0 ;
)
��; <
)
��< =
{
�� 
tags
�� 
=
�� 
new
�� 
List
�� 
<
��  
string
��  &
>
��& '
(
��' (
)
��( )
;
��) *
}
�� 
else
�� 
if
�� 
(
�� 
!
�� 
HasTags
�� 
(
�� 
DefaultTags
�� )
)
��) *
)
��* +
{
�� 
tags
�� 
=
�� 
TagNormalizer
�� $
.
��$ %
NormalizeTags
��% 2
(
��2 3
	givenTags
��3 <
)
��< =
;
��= >
}
�� 
else
�� 
if
�� 
(
�� 
!
�� 
HasTags
�� 
(
�� 
	givenTags
�� '
)
��' (
)
��( )
{
�� 
tags
�� 
=
�� 
new
�� 
List
�� 
<
��  
string
��  &
>
��& '
(
��' (
)
��( )
;
��) *
foreach
�� 
(
�� 
var
�� 
tag
��  
in
��! #
DefaultTags
��$ /
)
��/ 0
{
�� 
tags
�� 
.
�� 
Add
�� 
(
�� 
tag
��  
)
��  !
;
��! "
}
�� 
}
�� 
else
�� 
{
�� 
tags
�� 
=
�� 
new
�� 
List
�� 
<
��  
string
��  &
>
��& '
(
��' (
)
��( )
;
��) *
foreach
�� 
(
�� 
var
�� 
tag
��  
in
��! #
DefaultTags
��$ /
)
��/ 0
{
�� 
tags
�� 
.
�� 
Add
�� 
(
�� 
tag
��  
)
��  !
;
��! "
}
�� 
foreach
�� 
(
�� 
var
�� 
tag
��  
in
��! #
	givenTags
��$ -
)
��- .
{
�� 
tags
�� 
.
�� 
Add
�� 
(
�� 
tag
��  
)
��  !
;
��! "
}
�� 
tags
�� 
=
�� 
TagNormalizer
�� $
.
��$ %
NormalizeTags
��% 2
(
��2 3
tags
��3 7
)
��7 8
;
��8 9
}
�� 
if
�� 
(
�� 
hasException
�� 
)
�� 
{
�� 
tags
�� 
.
�� 
Add
�� 
(
�� 
exceptionTag
�� %
)
��% &
;
��& '
}
�� 
return
�� 
tags
�� 
;
�� 
}
�� 	
	protected
�� 
static
�� 
bool
�� 
HasTags
�� %
(
��% &
IEnumerable
��& 1
<
��1 2
string
��2 8
>
��8 9
tags
��: >
)
��> ?
{
�� 	
return
�� 
tags
�� 
!=
�� 
null
�� 
&&
��  "
tags
��# '
.
��' (
Count
��( -
(
��- .
)
��. /
>
��0 1
$num
��2 3
;
��3 4
}
�� 	
	protected
�� 
IDictionary
�� 
<
�� 
string
�� $
,
��$ %
object
��& ,
>
��, -
GetMetaData
��. 9
(
��9 :
IDictionary
��: E
<
��E F
string
��F L
,
��L M
object
��N T
>
��T U
givenMetaData
��V c
)
��c d
{
�� 	
var
�� 
metaData
�� 
=
�� 
new
�� 

Dictionary
�� )
<
��) *
string
��* 0
,
��0 1
object
��2 8
>
��8 9
(
��9 :
)
��: ;
;
��; <
AddMetaData
�� 
(
�� 
DefaultMetaData
�� '
,
��' (
metaData
��) 1
)
��1 2
;
��2 3
var
�� 
providedMetaData
��  
=
��! "
MetaDataProvider
��# 3
!=
��4 6
null
��7 ;
?
�� 
MetaDataProvider
�� "
.
��" #
ProvideMetaData
��# 2
(
��2 3
)
��3 4
:
�� 
null
�� 
;
�� 
AddMetaData
�� 
(
�� 
providedMetaData
�� (
,
��( )
metaData
��* 2
)
��2 3
;
��3 4
AddMetaData
�� 
(
�� 
givenMetaData
�� %
,
��% &
metaData
��' /
)
��/ 0
;
��0 1
return
�� 
metaData
�� 
;
�� 
}
�� 	
	protected
�� 
static
�� 
void
�� 
AddMetaData
�� )
(
��) *
IDictionary
��* 5
<
��5 6
string
��6 <
,
��< =
object
��> D
>
��D E
sourceMetaData
��F T
,
��T U
IDictionary
��V a
<
��a b
string
��b h
,
��h i
object
��j p
>
��p q
targetMetaData��r �
)��� �
{
�� 	
if
�� 
(
�� 
sourceMetaData
�� 
!=
�� !
null
��" &
)
��& '
{
�� 
foreach
�� 
(
�� 
var
�� 
item
�� !
in
��" $
sourceMetaData
��% 3
)
��3 4
{
�� 
targetMetaData
�� "
[
��" #
item
��# '
.
��' (
Key
��( +
]
��+ ,
=
��- .
item
��/ 3
.
��3 4
Value
��4 9
;
��9 :
}
�� 
}
�� 
}
�� 	
	protected
�� 
static
�� 
bool
�� 
HasMetaData
�� )
(
��) *
IDictionary
��* 5
<
��5 6
string
��6 <
,
��< =
object
��> D
>
��D E
metaData
��F N
)
��N O
{
�� 	
return
�� 
metaData
�� 
!=
�� 
null
�� #
&&
��$ &
metaData
��' /
.
��/ 0
Count
��0 5
>
��6 7
$num
��8 9
;
��9 :
}
�� 	
}
�� 
}�� �(
:C:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\LogManager.cs
	namespace

 	
NuLog


 
{ 
public 

static 
class 

LogManager "
{ 
private 
static 
readonly 
object  &
LogManagerLock' 5
=6 7
new8 ;
object< B
(B C
)C D
;D E
private 
static 
ILoggerFactory %
_loggerFactory& 4
;4 5
public!! 
static!! 
void!! 

SetFactory!! %
(!!% &
ILoggerFactory!!& 4
loggerFactory!!5 B
)!!B C
{"" 	
lock## 
(## 
LogManagerLock##  
)##  !
{$$ 
_loggerFactory%% 
=%%  
loggerFactory%%! .
;%%. /
}&& 
}'' 	
public,, 
static,, 
ILogger,, 
	GetLogger,, '
(,,' (
IMetaDataProvider,,( 9
metaDataProvider,,: J
=,,K L
null,,M Q
,,,Q R
params,,S Y
string,,Z `
[,,` a
],,a b
defaultTags,,c n
),,n o
{-- 	
var.. 
classTag.. 
=.. 
GetClassTag.. &
(..& '
)..' (
;..( )
var// 
factory// 
=// 
GetLoggerFactory// *
(//* +
)//+ ,
;//, -
return00 
factory00 
.00 
	GetLogger00 $
(00$ %
metaDataProvider00% 5
,005 6
GetDefaultTags007 E
(00E F
defaultTags00F Q
,00Q R
classTag00S [
)00[ \
)00\ ]
;00] ^
}11 	
public77 
static77 
void77 
Shutdown77 #
(77# $
)77$ %
{88 	
lock99 
(99 
LogManagerLock99  
)99  !
{:: 
if;; 
(;; 
_loggerFactory;; "
!=;;# %
null;;& *
);;* +
{<< 
_loggerFactory== "
.==" #
Dispose==# *
(==* +
)==+ ,
;==, -
_loggerFactory>> "
=>># $
null>>% )
;>>) *
}?? 
}@@ 
}AA 	
privateFF 
staticFF 
stringFF 
GetClassTagFF )
(FF) *
)FF* +
{GG 	
varHH 
methodHH 
=HH 
newHH 

StackFrameHH '
(HH' (
$numHH( )
)HH) *
.HH* +
	GetMethodHH+ 4
(HH4 5
)HH5 6
;HH6 7
returnII 
methodII 
.II 
ReflectedTypeII '
.II' (
FullNameII( 0
;II0 1
}JJ 	
privateOO 
staticOO 
IEnumerableOO "
<OO" #
stringOO# )
>OO) *
GetDefaultTagsOO+ 9
(OO9 :
IEnumerableOO: E
<OOE F
stringOOF L
>OOL M
	givenTagsOON W
,OOW X
stringOOY _
classTagOO` h
)OOh i
{PP 	
varQQ 
hashSetQQ 
=QQ 
newQQ 
HashSetQQ %
<QQ% &
stringQQ& ,
>QQ, -
(QQ- .
)QQ. /
;QQ/ 0
ifSS 
(SS 
	givenTagsSS 
!=SS 
nullSS !
)SS! "
{TT 
foreachUU 
(UU 
varUU 
tagUU  
inUU! #
	givenTagsUU$ -
)UU- .
{VV 
hashSetWW 
.WW 
AddWW 
(WW  
tagWW  #
)WW# $
;WW$ %
}XX 
}YY 
hashSet[[ 
.[[ 
Add[[ 
([[ 
classTag[[  
)[[  !
;[[! "
return]] 
hashSet]] 
;]] 
}^^ 	
private`` 
static`` 
ILoggerFactory`` %
GetLoggerFactory``& 6
(``6 7
)``7 8
{aa 	
ifbb 
(bb 
_loggerFactorybb 
==bb !
nullbb" &
)bb& '
{cc 
lockdd 
(dd 
LogManagerLockdd $
)dd$ %
{ee 
ifff 
(ff 
_loggerFactoryff &
==ff' )
nullff* .
)ff. /
{gg 
varhh 
configProviderhh *
=hh+ ,
newhh- 0(
ConfigurationManagerProviderhh1 M
(hhM N
)hhN O
;hhO P
varii 
configii "
=ii# $
configProviderii% 3
.ii3 4
GetConfigurationii4 D
(iiD E
)iiE F
;iiF G
_loggerFactoryjj &
=jj' (
newjj) ,!
StandardLoggerFactoryjj- B
(jjB C
configjjC I
)jjI J
;jjJ K
}kk 
}ll 
}mm 
returnoo 
_loggerFactoryoo !
;oo! "
}pp 	
}qq 
}rr �
GC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str  
)  !
]! "
[		 
assembly		 	
:			 

AssemblyDescription		 
(		 
$str		 !
)		! "
]		" #
[

 
assembly

 	
:

	 
!
AssemblyConfiguration

  
(

  !
$str

! #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str "
)" #
]# $
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
[## 
assembly## 	
:##	 

AssemblyVersion## 
(## 
$str## $
)##$ %
]##% &
[$$ 
assembly$$ 	
:$$	 

AssemblyFileVersion$$ 
($$ 
$str$$ (
)$$( )
]$$) *�
\C:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Loggers\TagNormalizers\StandardTagNormalizer.cs
	namespace		 	
NuLog		
 
{

 
public 

class !
StandardTagNormalizer &
:' (
ITagNormalizer) 7
{ 
private 
static 
readonly 
Regex  %
tagValPattern& 3
=4 5
new6 9
Regex: ?
(? @
$str@ R
,R S
RegexOptionsT `
.` a
Compileda i
|j k
RegexOptionsl x
.x y
CultureInvariant	y �
)
� �
;
� �
public 
string 
NormalizeTag "
(" #
string# )
tag* -
)- .
{ 	
if 
( 
string 
. 
IsNullOrEmpty $
($ %
tag% (
)( )
)) *
throw 
new %
InvalidOperationException 3
(3 4
$str4 z
)z {
;{ |
var 

normalized 
= 
tag  
.  !
Trim! %
(% &
)& '
;' (

normalized 
= 

normalized #
.# $
ToLower$ +
(+ ,
), -
;- .
ValidateTag"" 
("" 

normalized"" "
)""" #
;""# $
return%% 

normalized%% 
;%% 
}&& 	
public(( 
ICollection(( 
<(( 
string(( !
>((! "
NormalizeTags((# 0
(((0 1
IEnumerable((1 <
<((< =
string((= C
>((C D
tags((E I
)((I J
{)) 	
var** 
hashSet** 
=** 
new** 
HashSet** %
<**% &
string**& ,
>**, -
(**- .
)**. /
;**/ 0
if,, 
(,, 
tags,, 
!=,, 
null,, 
),, 
{-- 
foreach.. 
(.. 
var.. 
tag..  
in..! #
tags..$ (
)..( )
{// 
var00 
normalizedTag00 %
=00& '
NormalizeTag00( 4
(004 5
tag005 8
)008 9
;009 :
hashSet11 
.11 
Add11 
(11  
normalizedTag11  -
)11- .
;11. /
}22 
}33 
return55 
hashSet55 
;55 
}66 	
private== 
void== 
ValidateTag==  
(==  !
string==! '
tag==( +
)==+ ,
{>> 	
if?? 
(?? 
!?? 
tagValPattern?? 
.?? 
IsMatch?? &
(??& '
tag??' *
)??* +
)??+ ,
{@@ 
throwAA 
newAA %
InvalidOperationExceptionAA 3
(AA3 4
stringAA4 :
.AA: ;
FormatAA; A
(AAA B
$strAAB l
,AAl m
tagAAn q
)AAq r
)AAr s
;AAs t
}BB 
}CC 	
privateJJ 
voidJJ 
ValidateTagsJJ !
(JJ! "
stringJJ" (
[JJ( )
]JJ) *
tagsJJ+ /
)JJ/ 0
{KK 	
foreachLL 
(LL 
varLL 
tagLL 
inLL 
tagsLL  $
)LL$ %
{MM 
ValidateTagNN 
(NN 
tagNN 
)NN  
;NN  !
}OO 
}PP 	
}QQ 
}RR �
IC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\TagRouters\IRuleProcessor.cs
	namespace 	
NuLog
 
. 

TagRouters 
{ 
public 

	interface 
IRuleProcessor #
{ 
IEnumerable 
< 
string 
> 
DetermineTargets ,
(, -
IEnumerable- 8
<8 9
string9 ?
>? @
tagsA E
)E F
;F G
} 
} �
MC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\TagRouters\ITagGroupProcessor.cs
	namespace 	
NuLog
 
. 

TagRouters 
{ 
public 

	interface 
ITagGroupProcessor '
{ 
IEnumerable 
< 
string 
> 

GetAliases &
(& '
string' -
tag. 1
)1 2
;2 3
} 
} �H
_C:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\TagRouters\RuleProcessors\StandardRuleProcessor.cs
	namespace

 	
NuLog


 
.

 

TagRouters

 
.

 
RuleProcessors

 )
{ 
public 

class !
StandardRuleProcessor &
:' (
IRuleProcessor) 7
{ 
private 
readonly 
IEnumerable $
<$ %
Rule% )
>) *
rules+ 0
;0 1
private 
readonly 
ITagGroupProcessor +
tagGroupProcessor, =
;= >
private 
readonly 
IDictionary $
<$ %
string% +
,+ ,
Regex- 2
>2 3
ruleTagPatterns4 C
;C D
public## !
StandardRuleProcessor## $
(##$ %
IEnumerable##% 0
<##0 1
Rule##1 5
>##5 6
rules##7 <
,##< =
ITagGroupProcessor##> P
tagGroupProcessor##Q b
)##b c
{$$ 	
this%% 
.%% 
rules%% 
=%% 
rules%% 
??%% !
new%%" %
Rule%%& *
[%%* +
]%%+ ,
{%%- .
}%%/ 0
;%%0 1
this'' 
.'' 
tagGroupProcessor'' "
=''# $
tagGroupProcessor''% 6
;''6 7
this)) 
.)) 
ruleTagPatterns))  
=))! "
new))# &

Dictionary))' 1
<))1 2
string))2 8
,))8 9
Regex)): ?
>))? @
())@ A
)))A B
;))B C
}** 	
public,, 
IEnumerable,, 
<,, 
string,, !
>,,! "
DetermineTargets,,# 3
(,,3 4
IEnumerable,,4 ?
<,,? @
string,,@ F
>,,F G
tags,,H L
),,L M
{-- 	
var// 
targets// 
=// 
new// 
HashSet// %
<//% &
string//& ,
>//, -
(//- .
)//. /
;/// 0
foreach22 
(22 
var22 
rule22 
in22  
rules22! &
)22& '
{33 
if55 
(55 
IsExcludeMatch55 "
(55" #
rule55# '
,55' (
tags55) -
)55- .
)55. /
{66 
continue88 
;88 
}99 
if<< 
(<< 
IsIncludeMatch<< "
(<<" #
rule<<# '
,<<' (
tags<<) -
)<<- .
)<<. /
{== 
foreach?? 
(?? 
var??  
target??! '
in??( *
rule??+ /
.??/ 0
Targets??0 7
)??7 8
{@@ 
targetsAA 
.AA  
AddAA  #
(AA# $
targetAA$ *
)AA* +
;AA+ ,
}BB 
}CC 
ifFF 
(FF 
ruleFF 
.FF 
FinalFF 
)FF 
{GG 
breakII 
;II 
}JJ 
}KK 
returnNN 
targetsNN 
;NN 
}OO 	
privateVV 
boolVV 
IsIncludeMatchVV #
(VV# $
RuleVV$ (
ruleVV) -
,VV- .
IEnumerableVV/ :
<VV: ;
stringVV; A
>VVA B
tagsVVC G
)VVG H
{WW 	
varXX 
anyMatchXX 
=XX 
falseXX  
;XX  !
foreach[[ 
([[ 
var[[ 
ruleTag[[  
in[[! #
rule[[$ (
.[[( )
Include[[) 0
)[[0 1
{\\ 
var]] 
	ruleMatch]] 
=]] 
false]]  %
;]]% &
foreach`` 
(`` 
var`` 
tag``  
in``! #
tags``$ (
)``( )
{aa 
ifbb 
(bb 

IsTagMatchbb "
(bb" #
ruleTagbb# *
,bb* +
tagbb, /
)bb/ 0
)bb0 1
{cc 
anyMatchee  
=ee! "
trueee# '
;ee' (
	ruleMatchff !
=ff" #
trueff$ (
;ff( )
breakgg 
;gg 
}hh 
}ii 
ifll 
(ll 
	ruleMatchll 
&&ll  
rulell! %
.ll% &
StrictIncludell& 3
==ll4 6
falsell7 <
)ll< =
{mm 
returnoo 
trueoo 
;oo  
}pp 
elseqq 
ifqq 
(qq 
	ruleMatchqq "
==qq# %
falseqq& +
&&qq, .
ruleqq/ 3
.qq3 4
StrictIncludeqq4 A
)qqA B
{rr 
returnss 
falsess  
;ss  !
}tt 
}uu 
returnxx 
anyMatchxx 
;xx 
}yy 	
private
�� 
bool
�� 
IsExcludeMatch
�� #
(
��# $
Rule
��$ (
rule
��) -
,
��- .
IEnumerable
��/ :
<
��: ;
string
��; A
>
��A B
tags
��C G
)
��G H
{
�� 	
if
�� 
(
�� 
rule
�� 
.
�� 
Exclude
�� 
==
�� 
null
��  $
||
��% '
rule
��( ,
.
��, -
Exclude
��- 4
.
��4 5
Count
��5 :
(
��: ;
)
��; <
==
��= ?
$num
��@ A
)
��A B
{
�� 
return
�� 
false
�� 
;
�� 
}
�� 
foreach
�� 
(
�� 
var
�� 
ruleTag
��  
in
��! #
rule
��$ (
.
��( )
Exclude
��) 0
)
��0 1
{
�� 
foreach
�� 
(
�� 
var
�� 
tag
��  
in
��! #
tags
��$ (
)
��( )
{
�� 
if
�� 
(
�� 

IsTagMatch
�� "
(
��" #
ruleTag
��# *
,
��* +
tag
��, /
)
��/ 0
)
��0 1
{
�� 
return
�� 
true
�� #
;
��# $
}
�� 
}
�� 
}
�� 
return
�� 
false
�� 
;
�� 
}
�� 	
private
�� 
bool
�� 

IsTagMatch
�� 
(
��  
string
��  &
ruleTag
��' .
,
��. /
string
��0 6
tag
��7 :
)
��: ;
{
�� 	
var
�� 
ruleTagPattern
�� 
=
��  
GetRuleTagPattern
��! 2
(
��2 3
ruleTag
��3 :
)
��: ;
;
��; <
var
�� 
anyMatch
�� 
=
�� 
false
��  
;
��  !
foreach
�� 
(
�� 
var
�� 
alias
�� 
in
�� !
tagGroupProcessor
��" 3
.
��3 4

GetAliases
��4 >
(
��> ?
tag
��? B
)
��B C
)
��C D
{
�� 
if
�� 
(
�� 
ruleTagPattern
�� "
.
��" #
IsMatch
��# *
(
��* +
alias
��+ 0
)
��0 1
)
��1 2
{
�� 
anyMatch
�� 
=
�� 
true
�� #
;
��# $
break
�� 
;
�� 
}
�� 
}
�� 
return
�� 
anyMatch
�� 
;
�� 
}
�� 	
private
�� 
Regex
�� 
GetRuleTagPattern
�� '
(
��' (
string
��( .
ruleTag
��/ 6
)
��6 7
{
�� 	
if
�� 
(
�� 
ruleTagPatterns
�� 
.
��  
ContainsKey
��  +
(
��+ ,
ruleTag
��, 3
)
��3 4
==
��5 7
false
��8 =
)
��= >
{
�� 
var
�� 
regexRuleTag
��  
=
��! "
ruleTag
��# *
.
��* +
Replace
��+ 2
(
��2 3
$str
��3 6
,
��6 7
$str
��8 =
)
��= >
;
��> ?
regexRuleTag
�� 
=
�� 
ruleTag
�� &
.
��& '
Replace
��' .
(
��. /
$str
��/ 2
,
��2 3
$str
��4 8
)
��8 9
;
��9 :
var
�� 
ruleTagPattern
�� "
=
��# $
new
��% (
Regex
��) .
(
��. /
regexRuleTag
��/ ;
,
��; <
RegexOptions
��= I
.
��I J
CultureInvariant
��J Z
|
��[ \
RegexOptions
��] i
.
��i j

IgnoreCase
��j t
|
��u v
RegexOptions��w �
.��� �
Compiled��� �
)��� �
;��� �
ruleTagPatterns
�� 
[
��  
ruleTag
��  '
]
��' (
=
��) *
ruleTagPattern
��+ 9
;
��9 :
}
�� 
return
�� 
ruleTagPatterns
�� "
[
��" #
ruleTag
��# *
]
��* +
;
��+ ,
}
�� 	
}
�� 
}�� �
LC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\TagRouters\StandardTagRouter.cs
	namespace		 	
NuLog		
 
.		 

TagRouters		 
{

 
public 

class 
StandardTagRouter "
:# $

ITagRouter% /
{ 
private 
readonly 
IRuleProcessor '
ruleProcessor( 5
;5 6
private 
readonly 
IDictionary $
<$ %
string% +
,+ ,
IEnumerable- 8
<8 9
string9 ?
>? @
>@ A

routeCacheB L
;L M
public 
StandardTagRouter  
(  !
IRuleProcessor! /
ruleProcessor0 =
)= >
{ 	
this 
. 
ruleProcessor 
=  
ruleProcessor! .
;. /
this!! 
.!! 

routeCache!! 
=!! 
new!! !

Dictionary!!" ,
<!!, -
string!!- 3
,!!3 4
IEnumerable!!5 @
<!!@ A
string!!A G
>!!G H
>!!H I
(!!I J
)!!J K
;!!K L
}"" 	
public$$ 
IEnumerable$$ 
<$$ 
string$$ !
>$$! "
Route$$# (
($$( )
IEnumerable$$) 4
<$$4 5
string$$5 ;
>$$; <
tags$$= A
)$$A B
{%% 	
var'' 
cacheKey'' 
='' 
BuildTagsKey'' '
(''' (
tags''( ,
)'', -
;''- .
if** 
(** 

routeCache** 
.** 
ContainsKey** &
(**& '
cacheKey**' /
)**/ 0
==**1 3
false**4 9
)**9 :
{++ 

routeCache,, 
[,, 
cacheKey,, #
],,# $
=,,% &
this,,' +
.,,+ ,
ruleProcessor,,, 9
.,,9 :
DetermineTargets,,: J
(,,J K
tags,,K O
),,O P
;,,P Q
}-- 
return00 

routeCache00 
[00 
cacheKey00 &
]00& '
;00' (
}11 	
private66 
static66 
string66 
BuildTagsKey66 *
(66* +
IEnumerable66+ 6
<666 7
string667 =
>66= >
tags66? C
)66C D
{77 	
var88 
sb88 
=88 
new88 
StringBuilder88 &
(88& '
)88' (
;88( )
foreach99 
(99 
var99 
tag99 
in99 
tags99  $
)99$ %
{:: 
sb;; 
.;; 
Append;; 
(;; 
tag;; 
+;; 
$str;;  #
);;# $
;;;$ %
}<< 
return== 
sb== 
.== 
ToString== 
(== 
)==  
;==  !
}>> 	
}?? 
}@@ �
gC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\TagRouters\TagGroupProcessors\StandardTagGroupProcessor.cs
	namespace		 	
NuLog		
 
.		 

TagRouters		 
.		 
TagGroupProcessors		 -
{

 
public 

class %
StandardTagGroupProcessor *
:+ ,
ITagGroupProcessor- ?
{ 
private 
readonly 
IEnumerable $
<$ %
TagGroup% -
>- .
	tagGroups/ 8
;8 9
private 
readonly 
IDictionary $
<$ %
string% +
,+ ,
string- 3
[3 4
]4 5
>5 6

aliasCache7 A
;A B
public %
StandardTagGroupProcessor (
(( )
IEnumerable) 4
<4 5
TagGroup5 =
>= >
	tagGroups? H
)H I
{ 	
this 
. 
	tagGroups 
= 
	tagGroups &
;& '
this 
. 

aliasCache 
= 
new !

Dictionary" ,
<, -
string- 3
,3 4
string5 ;
[; <
]< =
>= >
(> ?
)? @
;@ A
} 	
public 
IEnumerable 
< 
string !
>! "

GetAliases# -
(- .
string. 4
tag5 8
)8 9
{ 	
if 
( 

aliasCache 
. 
ContainsKey &
(& '
tag' *
)* +
==, .
false/ 4
)4 5
{ 
var   
aliases   
=   
CalculateAliases   .
(  . /
tag  / 2
)  2 3
;  3 4

aliasCache!! 
[!! 
tag!! 
]!! 
=!!  !
aliases!!" )
.!!) *
ToArray!!* 1
(!!1 2
)!!2 3
;!!3 4
}"" 
return%% 

aliasCache%% 
[%% 
tag%% !
]%%! "
;%%" #
}&& 	
private(( 
IEnumerable(( 
<(( 
string(( "
>((" #
CalculateAliases(($ 4
(((4 5
string((5 ;
tag((< ?
)((? @
{)) 	
yield** 
return** 
tag** 
;** 
if,, 
(,, 
	tagGroups,, 
!=,, 
null,, !
),,! "
{-- 
foreach.. 
(.. 
var.. 
tagGroup.. %
in..& (
	tagGroups..) 2
)..2 3
{// 
foreach00 
(00 
var00  
alias00! &
in00' )
tagGroup00* 2
.002 3
Aliases003 :
)00: ;
{11 
if22 
(22 
alias22 !
==22" $
tag22% (
)22( )
{33 
yield44 !
return44" (
tagGroup44) 1
.441 2
BaseTag442 9
;449 :
continue55 $
;55$ %
}66 
}77 
}88 
}99 
}:: 	
};; 
}<< � 
EC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\ConsoleTarget.cs
	namespace		 	
NuLog		
 
.		 
Targets		 
{

 
public 

class 
ConsoleTarget 
:  
LayoutTargetBase! 1
{ 
private 
static 
readonly 
object  &
consoleLock' 2
=3 4
new5 8
object9 ?
(? @
)@ A
;A B
private 
static 
readonly 
Type  $
consoleColorType% 5
=6 7
typeof8 >
(> ?
ConsoleColor? K
)K L
;L M
private 
ConsoleColor 
backgroundColor ,
;, -
private"" 
ConsoleColor"" 
foregroundColor"" ,
;"", -
public$$ 
ConsoleTarget$$ 
($$ 
)$$ 
:$$  
base$$! %
($$% &
)$$& '
{%% 	
this&& 
.&& 
backgroundColor&&  
=&&! "
Console&&# *
.&&* +
BackgroundColor&&+ :
;&&: ;
this'' 
.'' 
foregroundColor''  
=''! "
Console''# *
.''* +
ForegroundColor''+ :
;'': ;
}(( 	
public** 
override** 
void** 
Write** "
(**" #
LogEvent**# +
logEvent**, 4
)**4 5
{++ 	
var,, 
message,, 
=,, 
Layout,,  
.,,  !
Format,,! '
(,,' (
logEvent,,( 0
),,0 1
;,,1 2
lock.. 
(.. 
consoleLock.. 
).. 
{// 
Console00 
.00 
BackgroundColor00 '
=00( )
backgroundColor00* 9
;009 :
Console11 
.11 
ForegroundColor11 '
=11( )
foregroundColor11* 9
;119 :
Console22 
.22 
Write22 
(22 
message22 %
)22% &
;22& '
Console33 
.33 

ResetColor33 "
(33" #
)33# $
;33$ %
}44 
}55 	
public77 
override77 
void77 
	Configure77 &
(77& '
TargetConfig77' 3
config774 :
)77: ;
{88 	
var:: 
bgColorName:: 
=:: 
GetProperty:: )
<::) *
string::* 0
>::0 1
(::1 2
config::2 8
,::8 9
$str::: F
)::F G
;::G H
if;; 
(;; 
string;; 
.;; 
IsNullOrEmpty;; $
(;;$ %
bgColorName;;% 0
);;0 1
==;;2 4
false;;5 :
);;: ;
{<< 
backgroundColor== 
===  !
(==" #
ConsoleColor==# /
)==/ 0
Enum==0 4
.==4 5
Parse==5 :
(==: ;
consoleColorType==; K
,==K L
bgColorName==M X
)==X Y
;==Y Z
}>> 
varAA 
fgColorNameAA 
=AA 
GetPropertyAA )
<AA) *
stringAA* 0
>AA0 1
(AA1 2
configAA2 8
,AA8 9
$strAA: F
)AAF G
;AAG H
ifBB 
(BB 
stringBB 
.BB 
IsNullOrEmptyBB $
(BB$ %
fgColorNameBB% 0
)BB0 1
==BB2 4
falseBB5 :
)BB: ;
{CC 
foregroundColorDD 
=DD  !
(DD" #
ConsoleColorDD# /
)DD/ 0
EnumDD0 4
.DD4 5
ParseDD5 :
(DD: ;
consoleColorTypeDD; K
,DDK L
fgColorNameDDM X
)DDX Y
;DDY Z
}EE 
baseHH 
.HH 
	ConfigureHH 
(HH 
configHH !
)HH! "
;HH" #
}II 	
}JJ 
}KK �
FC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\ILayoutFactory.cs
	namespace 	
NuLog
 
. 
Targets 
{ 
public

 

	interface

 
ILayoutFactory

 #
{ 
ILayout 
	GetLayout 
( 
string  
format! '
)' (
;( )
} 
} �
EC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\ILayoutTarget.cs
	namespace 	
NuLog
 
. 
Targets 
{ 
public 

	interface 
ILayoutTarget "
:# $
ITarget% ,
{ 
void 
	Configure 
( 
TargetConfig #
config$ *
,* +
ILayoutFactory, :
layoutFactory; H
)H I
;I J
} 
} �

CC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\TraceTarget.cs
	namespace		 	
NuLog		
 
.		 
Targets		 
{

 
public 

class 
TraceTarget 
: 
LayoutTargetBase /
{ 
public 
override 
void 
Write "
(" #
LogEvent# +
logEvent, 4
)4 5
{ 	
try 
{ 
var 
	formatted 
= 
this  $
.$ %
Layout% +
.+ ,
Format, 2
(2 3
logEvent3 ;
); <
;< =
Trace 
. 
Write 
( 
	formatted %
)% &
;& '
} 
catch 
( "
NullReferenceException )
cause* /
)/ 0
{ 
if 
( 
this 
. 
Layout 
==  "
null# '
)' (
{ 
throw 
new %
InvalidOperationException 7
(7 8
$str	8 �
,
� �
cause
� �
)
� �
;
� �
} 
throw   
;   
}!! 
}"" 	
}## 
}$$ �
DC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\EventLogShim.cs
	namespace 	
NuLog
 
. 
Targets 
{ 
public 

class 
EventLogShim 
: 
	IEventLog  )
{ 
public 
void 
CreateEventSource %
(% &
string& ,
source- 3
,3 4
string5 ;
logName< C
)C D
{ 	
EventLog 
. 
CreateEventSource &
(& '
source' -
,- .
logName/ 6
)6 7
;7 8
} 	
public 
bool 
SourceExists  
(  !
string! '
source( .
). /
{ 	
return 
EventLog 
. 
SourceExists (
(( )
source) /
)/ 0
;0 1
} 	
public 
void 

WriteEntry 
( 
string %
source& ,
,, -
string. 4
message5 <
,< =
EventLogEntryType> O
typeP T
)T U
{ 	

WriteEntry 
( 
source 
, 
message &
,& '
type( ,
), -
;- .
} 	
} 
} �#
FC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\EventLogTarget.cs
	namespace

 	
NuLog


 
.

 
Targets

 
{ 
public 

class 
EventLogTarget 
:  !
LayoutTargetBase" 2
{ 
private 
static 
readonly 
Type  $!
EventLogEntryTypeType% :
=; <
typeof= C
(C D
EventLogEntryTypeD U
)U V
;V W
private 
readonly 
	IEventLog "
eventLog# +
;+ ,
private 
string 
source 
; 
private 
EventLogEntryType !
	entryType" +
;+ ,
public 
EventLogTarget 
( 
) 
{ 	
eventLog 
= 
new 
EventLogShim '
(' (
)( )
;) *
} 	
public 
EventLogTarget 
( 
	IEventLog '
eventLog( 0
)0 1
{ 	
this   
.   
eventLog   
=   
eventLog   $
;  $ %
}!! 	
public## 
override## 
void## 
Write## "
(##" #
LogEvent### +
logEvent##, 4
)##4 5
{$$ 	
var%% 
message%% 
=%% 
Layout%%  
.%%  !
Format%%! '
(%%' (
logEvent%%( 0
)%%0 1
;%%1 2
eventLog&& 
.&& 

WriteEntry&& 
(&&  
source&&  &
,&&& '
message&&( /
,&&/ 0
	entryType&&1 :
)&&: ;
;&&; <
}'' 	
public)) 
override)) 
void)) 
	Configure)) &
())& '
TargetConfig))' 3
config))4 :
))): ;
{** 	
source,, 
=,, 
GetProperty,,  
<,,  !
string,,! '
>,,' (
(,,( )
config,,) /
,,,/ 0
$str,,1 9
),,9 :
;,,: ;
if-- 
(-- 
string-- 
.-- 
IsNullOrEmpty-- $
(--$ %
source--% +
)--+ ,
)--, -
{.. 
throw// 
new// %
InvalidOperationException// 3
(//3 4
$str//4 b
)//b c
;//c d
}00 
var33 
	sourceLog33 
=33 
GetProperty33 '
<33' (
string33( .
>33. /
(33/ 0
config330 6
,336 7
$str338 C
)33C D
;33D E
if44 
(44 
string44 
.44 
IsNullOrEmpty44 $
(44$ %
	sourceLog44% .
)44. /
)44/ 0
{55 
	sourceLog66 
=66 
$str66 )
;66) *
}77 
if:: 
(:: 
this:: 
.:: 
eventLog:: 
.:: 
SourceExists:: *
(::* +
source::+ 1
)::1 2
==::3 5
false::6 ;
)::; <
{;; 
this<< 
.<< 
eventLog<< 
.<< 
CreateEventSource<< /
(<</ 0
source<<0 6
,<<6 7
	sourceLog<<8 A
)<<A B
;<<B C
}== 
var@@ 
entryTypeRaw@@ 
=@@ 
GetProperty@@ *
<@@* +
string@@+ 1
>@@1 2
(@@2 3
config@@3 9
,@@9 :
$str@@; F
)@@F G
;@@G H
EventLogEntryTypeLL 
	entryTypeLL '
;LL' (
ifMM 
(MM 
EnumMM 
.MM 
TryParseMM 
(MM 
entryTypeRawMM *
,MM* +
outMM, /
	entryTypeMM0 9
)MM9 :
)MM: ;
{NN 
thisOO 
.OO 
	entryTypeOO 
=OO  
	entryTypeOO! *
;OO* +
}PP 
elseQQ 
{RR 
thisSS 
.SS 
	entryTypeSS 
=SS  
EventLogEntryTypeSS! 2
.SS2 3
InformationSS3 >
;SS> ?
}TT 
baseXX 
.XX 
	ConfigureXX 
(XX 
configXX !
)XX! "
;XX" #
}YY 	
}ZZ 
}[[ �
AC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\IEventLog.cs
	namespace 	
NuLog
 
. 
Targets 
{ 
public 

	interface 
	IEventLog 
{ 
void 

WriteEntry 
( 
string 
source %
,% &
string' -
message. 5
,5 6
EventLogEntryType7 H
typeI M
)M N
;N O
bool 
SourceExists 
( 
string  
source! '
)' (
;( )
void 
CreateEventSource 
( 
string %
source& ,
,, -
string. 4
logName5 <
)< =
;= >
} 
} �
?C:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\ILayout.cs
	namespace 	
NuLog
 
. 
Targets 
{ 
public 

	interface 
ILayout 
{ 
string 
Format 
( 
LogEvent 
logEvent '
)' (
;( )
} 
} �
CC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\ISmtpClient.cs
	namespace 	
NuLog
 
. 
Targets 
{		 
public 

	interface 
ISmtpClient  
:! "
IDisposable# .
{ 
void 
SetCredentials 
( 
string "
userName# +
,+ ,
string- 3
password4 <
)< =
;= >
void 
SetEnableSsl 
( 
bool 
	enableSsl (
)( )
;) *
void 
SetSmtpServer 
( 
string !

smtpServer" ,
), -
;- .
void!! 
SetSmtpPort!! 
(!! 
int!! 
port!! !
)!!! "
;!!" #
void&& !
SetSmtpDeliveryMethod&& "
(&&" #
SmtpDeliveryMethod&&# 5
smtpDeliveryMethod&&6 H
)&&H I
;&&I J
void++ &
SetPickupDirectoryLocation++ '
(++' (
string++( .#
pickupDirectoryLocation++/ F
)++F G
;++G H
void00 

SetTimeout00 
(00 
int00 
timeout00 #
)00# $
;00$ %
void55 
Send55 
(55 
MailMessage55 
mailMessage55 )
)55) *
;55* +
}66 
}77 �
HC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\LayoutTargetBase.cs
	namespace 	
NuLog
 
. 
Targets 
{ 
public 

abstract 
class 
LayoutTargetBase *
:+ ,

TargetBase- 7
,7 8
ILayoutTarget9 F
{ 
	protected 
ILayout 
Layout  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
void 
	Configure 
( 
TargetConfig *
config+ 1
,1 2
ILayoutFactory3 A
layoutFactoryB O
)O P
{ 	
var 
format 
= 
config 
!=  "
null# '
&&( *
config+ 1
.1 2

Properties2 <
!== ?
null@ D
&&E G
configH N
.N O

PropertiesO Y
.Y Z
ContainsKeyZ e
(e f
$strf n
)n o
? 
( 
string 
) 
config  
.  !

Properties! +
[+ ,
$str, 4
]4 5
: 
null 
; 
var 
layout 
= 
layoutFactory &
.& '
	GetLayout' 0
(0 1
format1 7
)7 8
;8 9
this 
. 
Layout 
= 
layout  
;  !
} 	
} 
} �x
BC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\MailTarget.cs
	namespace

 	
NuLog


 
.

 
Targets

 
{ 
public 

class 

MailTarget 
: 

TargetBase (
,( )
ILayoutTarget* 7
{ 
private 
static 
readonly 
Type  $"
SmtpDeliveryMethodType% ;
=< =
typeof> D
(D E
SmtpDeliveryMethodE W
)W X
;X Y
private 
const 
string #
DefaultBodyLayoutFormat 4
=5 6
$str	7 �
;
� �
private 
readonly 
ISmtpClient $

smtpClient% /
;/ 0
public 
ILayout 

BodyLayout !
{" #
get$ '
;' (
set) ,
;, -
}. /
public"" 
ILayout"" 
SubjectLayout"" $
{""% &
get""' *
;""* +
set"", /
;""/ 0
}""1 2
private'' 
bool'' 

bodyIsHtml'' 
;''  
private,, 
bool,,  
convertNewlineInHtml,, )
;,,) *
private11 
string11 
[11 
]11 
to11 
;11 
private66 
MailAddress66 
from66  
;66  !
public;; 
bool;; &
DisposeSmtpClientOnDispose;; .
{;;/ 0
get;;1 4
;;;4 5
set;;6 9
;;;9 :
};;; <
public== 

MailTarget== 
(== 
)== 
{>> 	
this?? 
.?? 

smtpClient?? 
=?? 
new?? !
SmtpClientShim??" 0
(??0 1
)??1 2
;??2 3&
DisposeSmtpClientOnDispose@@ &
=@@' (
true@@) -
;@@- .
}AA 	
publicCC 

MailTargetCC 
(CC 
ISmtpClientCC %

smtpClientCC& 0
)CC0 1
{DD 	
thisEE 
.EE 

smtpClientEE 
=EE 

smtpClientEE (
;EE( )
}FF 	
publicHH 
overrideHH 
voidHH 
WriteHH "
(HH" #
LogEventHH# +
logEventHH, 4
)HH4 5
{II 	
varJJ 
bodyJJ 
=JJ 

FormatBodyJJ !
(JJ! "
logEventJJ" *
)JJ* +
;JJ+ ,
varKK 
subjectKK 
=KK 
SubjectLayoutKK '
.KK' (
FormatKK( .
(KK. /
logEventKK/ 7
)KK7 8
;KK8 9
varNN 
messageNN 
=NN 
newNN 
MailMessageNN )
{OO 
BodyPP 
=PP 
bodyPP 
,PP 
SubjectQQ 
=QQ 
subjectQQ !
,QQ! "

IsBodyHtmlRR 
=RR 

bodyIsHtmlRR '
}SS 
;SS 
ifVV 
(VV 
fromVV 
!=VV 
nullVV 
)VV 
{WW 
messageXX 
.XX 
FromXX 
=XX 
fromXX #
;XX# $
}YY 
foreach\\ 
(\\ 
var\\ 
	addressee\\ "
in\\# %
to\\& (
)\\( )
{]] 
message^^ 
.^^ 
To^^ 
.^^ 
Add^^ 
(^^ 
	addressee^^ (
)^^( )
;^^) *
}__ 

smtpClientbb 
.bb 
Sendbb 
(bb 
messagebb #
)bb# $
;bb$ %
}cc 	
privatehh 
stringhh 

FormatBodyhh !
(hh! "
LogEventhh" *
logEventhh+ 3
)hh3 4
{ii 	
varjj 
bodyjj 
=jj 

BodyLayoutjj !
.jj! "
Formatjj" (
(jj( )
logEventjj) 1
)jj1 2
;jj2 3
ifkk 
(kk 

bodyIsHtmlkk 
&&kk  
convertNewlineInHtmlkk 2
)kk2 3
{ll 
bodymm 
=mm 
bodymm 
.mm 
Replacemm #
(mm# $
$strmm$ *
,mm* +
$strmm, 4
)mm4 5
;mm5 6
}nn 
returnoo 
bodyoo 
;oo 
}pp 	
publicrr 
overriderr 
voidrr 
	Configurerr &
(rr& '
TargetConfigrr' 3
configrr4 :
)rr: ;
{ss 	
varuu 
htmlFlagRawuu 
=uu 
GetPropertyuu )
<uu) *
stringuu* 0
>uu0 1
(uu1 2
configuu2 8
,uu8 9
$struu: @
)uu@ A
;uuA B
boolvv 
htmlFlagvv 
;vv 
ifww 
(ww 
boolww 
.ww 
TryParseww 
(ww 
htmlFlagRawww )
,ww) *
outww+ .
htmlFlagww/ 7
)ww7 8
)ww8 9
{xx 

bodyIsHtmlyy 
=yy 
htmlFlagyy %
;yy% &
}zz 
var}} '
convertNewlineInHtmlFlagRaw}} +
=}}, -
GetProperty}}. 9
<}}9 :
string}}: @
>}}@ A
(}}A B
config}}B H
,}}H I
$str}}J `
)}}` a
;}}a b
bool~~ #
convertNewlinInHtmlFlag~~ (
;~~( )
if 
( 
bool 
. 
TryParse 
( '
convertNewlineInHtmlFlagRaw 9
,9 :
out; >#
convertNewlinInHtmlFlag? V
)V W
)W X
{
�� "
convertNewlineInHtml
�� $
=
��% &%
convertNewlinInHtmlFlag
��' >
;
��> ?
}
�� 
var
�� 
recipientsString
��  
=
��! "
GetProperty
��# .
<
��. /
string
��/ 5
>
��5 6
(
��6 7
config
��7 =
,
��= >
$str
��? C
)
��C D
;
��D E
if
�� 
(
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� $
(
��$ %
recipientsString
��% 5
)
��5 6
==
��7 9
false
��: ?
)
��? @
{
�� 
to
�� 
=
�� 
recipientsString
�� %
.
��% &
Split
��& +
(
��+ ,
$char
��, /
)
��/ 0
;
��0 1
}
�� 
var
�� 

fromString
�� 
=
�� 
GetProperty
�� (
<
��( )
string
��) /
>
��/ 0
(
��0 1
config
��1 7
,
��7 8
$str
��9 ?
)
��? @
;
��@ A
if
�� 
(
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� $
(
��$ %

fromString
��% /
)
��/ 0
==
��1 3
false
��4 9
)
��9 :
{
�� 
from
�� 
=
�� 
new
�� 
MailAddress
�� &
(
��& '

fromString
��' 1
)
��1 2
;
��2 3
}
�� 
var
�� 
userNameString
�� 
=
��  
GetProperty
��! ,
<
��, -
string
��- 3
>
��3 4
(
��4 5
config
��5 ;
,
��; <
$str
��= K
)
��K L
;
��L M
var
�� 
password
�� 
=
�� 
GetProperty
�� &
<
��& '
string
��' -
>
��- .
(
��. /
config
��/ 5
,
��5 6
$str
��7 E
)
��E F
;
��F G

smtpClient
�� 
.
�� 
SetCredentials
�� %
(
��% &
userNameString
��& 4
,
��4 5
password
��6 >
)
��> ?
;
��? @
var
�� 
enableSslFlagRaw
��  
=
��! "
GetProperty
��# .
<
��. /
string
��/ 5
>
��5 6
(
��6 7
config
��7 =
,
��= >
$str
��? J
)
��J K
;
��K L
bool
�� 
enableSslFlag
�� 
;
�� 
if
�� 
(
�� 
bool
�� 
.
�� 
TryParse
�� 
(
�� 
enableSslFlagRaw
�� .
,
��. /
out
��0 3
enableSslFlag
��4 A
)
��A B
)
��B C
{
�� 

smtpClient
�� 
.
�� 
SetEnableSsl
�� '
(
��' (
enableSslFlag
��( 5
)
��5 6
;
��6 7
}
�� 
var
�� 

smtpServer
�� 
=
�� 
GetProperty
�� (
<
��( )
string
��) /
>
��/ 0
(
��0 1
config
��1 7
,
��7 8
$str
��9 E
)
��E F
;
��F G
if
�� 
(
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� $
(
��$ %

smtpServer
��% /
)
��/ 0
==
��1 3
false
��4 9
)
��9 :
{
�� 

smtpClient
�� 
.
�� 
SetSmtpServer
�� (
(
��( )

smtpServer
��) 3
)
��3 4
;
��4 5
}
�� 
var
�� 
smtpPortRaw
�� 
=
�� 
GetProperty
�� )
<
��) *
string
��* 0
>
��0 1
(
��1 2
config
��2 8
,
��8 9
$str
��: D
)
��D E
;
��E F
int
�� 
smtpPort
�� 
;
�� 
if
�� 
(
�� 
int
�� 
.
�� 
TryParse
�� 
(
�� 
smtpPortRaw
�� (
,
��( )
out
��* -
smtpPort
��. 6
)
��6 7
)
��7 8
{
�� 

smtpClient
�� 
.
�� 
SetSmtpPort
�� &
(
��& '
smtpPort
��' /
)
��/ 0
;
��0 1
}
�� 
var
�� #
smtpDeliveryMethodRaw
�� %
=
��& '
GetProperty
��( 3
<
��3 4
string
��4 :
>
��: ;
(
��; <
config
��< B
,
��B C
$str
��D X
)
��X Y
;
��Y Z
if
�� 
(
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� $
(
��$ %#
smtpDeliveryMethodRaw
��% :
)
��: ;
==
��< >
false
��? D
)
��D E
{
�� 
var
��  
smtpDeliveryMethod
�� &
=
��' (
(
��) * 
SmtpDeliveryMethod
��* <
)
��< =
Enum
��= A
.
��A B
Parse
��B G
(
��G H$
SmtpDeliveryMethodType
��H ^
,
��^ _#
smtpDeliveryMethodRaw
��` u
)
��u v
;
��v w

smtpClient
�� 
.
�� #
SetSmtpDeliveryMethod
�� 0
(
��0 1 
smtpDeliveryMethod
��1 C
)
��C D
;
��D E
}
�� 
var
�� %
pickupDirectoryLocation
�� '
=
��( )
GetProperty
��* 5
<
��5 6
string
��6 <
>
��< =
(
��= >
config
��> D
,
��D E
$str
��F _
)
��_ `
;
��` a
if
�� 
(
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� $
(
��$ %%
pickupDirectoryLocation
��% <
)
��< =
==
��> @
false
��A F
)
��F G
{
�� 

smtpClient
�� 
.
�� (
SetPickupDirectoryLocation
�� 5
(
��5 6%
pickupDirectoryLocation
��6 M
)
��M N
;
��N O
}
�� 
var
�� 

timeoutRaw
�� 
=
�� 
GetProperty
�� (
<
��( )
string
��) /
>
��/ 0
(
��0 1
config
��1 7
,
��7 8
$str
��9 B
)
��B C
;
��C D
int
�� 
timeout
�� 
;
�� 
if
�� 
(
�� 
int
�� 
.
�� 
TryParse
�� 
(
�� 

timeoutRaw
�� '
,
��' (
out
��) ,
timeout
��- 4
)
��4 5
)
��5 6
{
�� 

smtpClient
�� 
.
�� 

SetTimeout
�� %
(
��% &
timeout
��& -
)
��- .
;
��. /
}
�� 
base
�� 
.
�� 
	Configure
�� 
(
�� 
config
�� !
)
��! "
;
��" #
}
�� 	
public
�� 
void
�� 
	Configure
�� 
(
�� 
TargetConfig
�� *
config
��+ 1
,
��1 2
ILayoutFactory
��3 A
layoutFactory
��B O
)
��O P
{
�� 	
var
�� 

bodyFormat
�� 
=
�� 
GetProperty
�� (
<
��( )
string
��) /
>
��/ 0
(
��0 1
config
��1 7
,
��7 8
$str
��9 ?
)
��? @
;
��@ A
if
�� 
(
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� $
(
��$ %

bodyFormat
��% /
)
��/ 0
)
��0 1
{
�� 

bodyFormat
�� 
=
�� %
DefaultBodyLayoutFormat
�� 4
;
��4 5
}
�� 
this
�� 
.
�� 

BodyLayout
�� 
=
�� 
layoutFactory
�� +
.
��+ ,
	GetLayout
��, 5
(
��5 6

bodyFormat
��6 @
)
��@ A
;
��A B
var
�� 
subjectFormat
�� 
=
�� 
GetProperty
��  +
<
��+ ,
string
��, 2
>
��2 3
(
��3 4
config
��4 :
,
��: ;
$str
��< E
)
��E F
;
��F G
if
�� 
(
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� $
(
��$ %
subjectFormat
��% 2
)
��2 3
==
��4 6
false
��7 <
)
��< =
{
�� 
this
�� 
.
�� 
SubjectLayout
�� "
=
��# $
layoutFactory
��% 2
.
��2 3
	GetLayout
��3 <
(
��< =
subjectFormat
��= J
)
��J K
;
��K L
}
�� 
else
�� 
{
�� 
throw
�� 
new
�� '
InvalidOperationException
�� 3
(
��3 4
$str��4 �
)��� �
;��� �
}
�� 
}
�� 	
public
�� 
override
�� 
void
�� 
Dispose
�� $
(
��$ %
)
��% &
{
�� 	
if
�� 
(
�� (
DisposeSmtpClientOnDispose
�� *
)
��* +
{
�� 

smtpClient
�� 
.
�� 
Dispose
�� "
(
��" #
)
��# $
;
��$ %
}
�� 
base
�� 
.
�� 
Dispose
�� 
(
�� 
)
�� 
;
�� 
}
�� 	
}
�� 
}�� �
FC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\SmtpClientShim.cs
	namespace 	
NuLog
 
. 
Targets 
{		 
public 

class 
SmtpClientShim 
:  !
ISmtpClient" -
{ 
private 
readonly 

SmtpClient #

smtpClient$ .
;. /
public 
SmtpClientShim 
( 
) 
{ 	
this 
. 

smtpClient 
= 
new !

SmtpClient" ,
(, -
)- .
;. /
} 	
public 
void 
Dispose 
( 
) 
{ 	
this 
. 

smtpClient 
. 
Dispose #
(# $
)$ %
;% &
} 	
public   
void   
Send   
(   
MailMessage   $
mailMessage  % 0
)  0 1
{!! 	
this"" 
."" 

smtpClient"" 
."" 
Send""  
(""  !
mailMessage""! ,
)"", -
;""- .
}## 	
public%% 
void%% 
SetCredentials%% "
(%%" #
string%%# )
userName%%* 2
,%%2 3
string%%4 :
password%%; C
)%%C D
{&& 	
this'' 
.'' 

smtpClient'' 
.'' 
Credentials'' '
=''( )
new''* -
NetworkCredential''. ?
(''? @
userName''@ H
,''H I
password''J R
)''R S
;''S T
}(( 	
public** 
void** 
SetEnableSsl**  
(**  !
bool**! %
	enableSsl**& /
)**/ 0
{++ 	
this,, 
.,, 

smtpClient,, 
.,, 
	EnableSsl,, %
=,,& '
	enableSsl,,( 1
;,,1 2
}-- 	
public// 
void// &
SetPickupDirectoryLocation// .
(//. /
string/// 5#
pickupDirectoryLocation//6 M
)//M N
{00 	
this11 
.11 

smtpClient11 
.11 #
PickupDirectoryLocation11 3
=114 5#
pickupDirectoryLocation116 M
;11M N
}22 	
public44 
void44 !
SetSmtpDeliveryMethod44 )
(44) *
SmtpDeliveryMethod44* <
smtpDeliveryMethod44= O
)44O P
{55 	
this66 
.66 

smtpClient66 
.66 
DeliveryMethod66 *
=66+ ,
smtpDeliveryMethod66- ?
;66? @
}77 	
public99 
void99 
SetSmtpPort99 
(99  
int99  #
port99$ (
)99( )
{:: 	
this;; 
.;; 

smtpClient;; 
.;; 
Port;;  
=;;! "
port;;# '
;;;' (
}<< 	
public>> 
void>> 
SetSmtpServer>> !
(>>! "
string>>" (

smtpServer>>) 3
)>>3 4
{?? 	
this@@ 
.@@ 

smtpClient@@ 
.@@ 
Host@@  
=@@! "

smtpServer@@# -
;@@- .
}AA 	
publicCC 
voidCC 

SetTimeoutCC 
(CC 
intCC "
timeoutCC# *
)CC* +
{DD 	
thisEE 
.EE 

smtpClientEE 
.EE 
TimeoutEE #
=EE$ %
timeoutEE& -
;EE- .
}FF 	
}GG 
}HH �
BC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\TargetBase.cs
	namespace 	
NuLog
 
. 
Targets 
{		 
public 

abstract 
class 

TargetBase $
:% &
ITarget' .
{ 
public 
virtual 
string 
Name "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
virtual 
void 
	Configure %
(% &
TargetConfig& 2
config3 9
)9 :
{ 	
} 	
public 
virtual 
void 
Dispose #
(# $
)$ %
{ 	
} 	
public 
abstract 
void 
Write "
(" #
LogEvent# +
logEvent, 4
)4 5
;5 6
	protected!! 
	TProperty!! 
GetProperty!! '
<!!' (
	TProperty!!( 1
>!!1 2
(!!2 3
TargetConfig!!3 ?
config!!@ F
,!!F G
string!!H N
propertyName!!O [
)!![ \
{"" 	
if$$ 
($$ 
config$$ 
.$$ 

Properties$$ !
==$$" $
null$$% )
)$$) *
{%% 
return&& 
default&& 
(&& 
	TProperty&& (
)&&( )
;&&) *
}'' 
if** 
(** 
config** 
.** 

Properties** !
.**! "
ContainsKey**" -
(**- .
propertyName**. :
)**: ;
==**< >
false**? D
)**D E
{++ 
return,, 
default,, 
(,, 
	TProperty,, (
),,( )
;,,) *
}-- 
var00 
property00 
=00 
config00 !
.00! "

Properties00" ,
[00, -
propertyName00- 9
]009 :
;00: ;
return11 
(11 
	TProperty11 
)11 
property11 &
;11& '
}22 	
	protected77 
bool77 
TryGetProperty77 %
<77% &
	TProperty77& /
>77/ 0
(770 1
TargetConfig771 =
config77> D
,77D E
string77F L
propertyName77M Y
,77Y Z
out77[ ^
	TProperty77_ h
property77i q
)77q r
{88 	
if:: 
(:: 
config:: 
.:: 

Properties:: !
.::! "
ContainsKey::" -
(::- .
propertyName::. :
)::: ;
==::< >
false::? D
)::D E
{;; 
property<< 
=<< 
default<< "
(<<" #
	TProperty<<# ,
)<<, -
;<<- .
return== 
false== 
;== 
}>> 
varAA 
objAA 
=AA 
configAA 
.AA 

PropertiesAA '
[AA' (
propertyNameAA( 4
]AA4 5
;AA5 6
varBB 

targetTypeBB 
=BB 
typeofBB #
(BB# $
	TPropertyBB$ -
)BB- .
;BB. /
ifCC 
(CC 
!CC 

targetTypeCC 
.CC 
IsAssignableFromCC ,
(CC, -
objCC- 0
.CC0 1
GetTypeCC1 8
(CC8 9
)CC9 :
)CC: ;
)CC; <
{DD 
propertyEE 
=EE 
defaultEE "
(EE" #
	TPropertyEE# ,
)EE, -
;EE- .
returnFF 
falseFF 
;FF 
}GG 
propertyJJ 
=JJ 
(JJ 
	TPropertyJJ !
)JJ! "
objJJ" %
;JJ% &
returnKK 
trueKK 
;KK 
}LL 	
}MM 
}NN �
FC:\Users\ivan\Source\Repos\NuLog\Take2\NuLog\Targets\TextFileTarget.cs
	namespace		 	
NuLog		
 
.		 
Targets		 
{

 
public 

class 
TextFileTarget 
:  !
LayoutTargetBase" 2
{ 
private 
string 
filePath 
;  
public 
override 
void 
Write "
(" #
LogEvent# +
logEvent, 4
)4 5
{ 	
var 
message 
= 
Layout  
.  !
Format! '
(' (
logEvent( 0
)0 1
;1 2
File 
. 
AppendAllText 
( 
filePath '
,' (
message) 0
)0 1
;1 2
} 	
public 
override 
void 
	Configure &
(& '
TargetConfig' 3
config4 :
): ;
{ 	
filePath 
= 
GetProperty "
<" #
string# )
>) *
(* +
config+ 1
,1 2
$str3 9
)9 :
;: ;
base   
.   
	Configure   
(   
config   !
)  ! "
;  " #
}!! 	
}"" 
}## 