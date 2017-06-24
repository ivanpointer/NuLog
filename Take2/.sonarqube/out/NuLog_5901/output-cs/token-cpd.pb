œ
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
}// ¡	
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
}'' à
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
} ø
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
} Ω
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
}&& é
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
} é~
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
ÄÄ 	
return
ÅÅ 
new
ÅÅ 
TargetConfig
ÅÅ #
{
ÇÇ 
Name
ÉÉ 
=
ÉÉ  
GetStringAttribute
ÉÉ )
(
ÉÉ) *

xmlElement
ÉÉ* 4
,
ÉÉ4 5
$str
ÉÉ6 <
)
ÉÉ< =
,
ÉÉ= >
Type
ÑÑ 
=
ÑÑ  
GetStringAttribute
ÑÑ )
(
ÑÑ) *

xmlElement
ÑÑ* 4
,
ÑÑ4 5
$str
ÑÑ6 <
)
ÑÑ< =
,
ÑÑ= >

Properties
ÖÖ 
=
ÖÖ '
GetAttributesAsDictionary
ÖÖ 6
(
ÖÖ6 7

xmlElement
ÖÖ7 A
)
ÖÖA B
}
ÜÜ 
;
ÜÜ 
}
áá 	
private
êê 
static
êê 
ICollection
êê "
<
êê" #
TagGroupConfig
êê# 1
>
êê1 2
ParseTagGroups
êê3 A
(
êêA B

XmlElement
êêB L

xmlElement
êêM W
)
êêW X
{
ëë 	
var
ìì 
	tagGroups
ìì 
=
ìì 
new
ìì 
List
ìì  $
<
ìì$ %
TagGroupConfig
ìì% 3
>
ìì3 4
(
ìì4 5
)
ìì5 6
;
ìì6 7
var
ññ 
tagGroupsElement
ññ  
=
ññ! "

xmlElement
ññ# -
.
ññ- .
SelectSingleNode
ññ. >
(
ññ> ?
$str
ññ? J
)
ññJ K
;
ññK L
if
ôô 
(
ôô 
tagGroupsElement
ôô  
==
ôô! #
null
ôô$ (
)
ôô( )
{
öö 
return
õõ 
	tagGroups
õõ  
;
õõ  !
}
úú 
foreach
üü 
(
üü 
var
üü 
tagGroupElement
üü (
in
üü) +
tagGroupsElement
üü, <
.
üü< =
SelectNodes
üü= H
(
üüH I
$str
üüI P
)
üüP Q
)
üüQ R
{
†† 
	tagGroups
°° 
.
°° 
Add
°° 
(
°° 
ParseTagGroup
°° +
(
°°+ ,
(
°°, -

XmlElement
°°- 7
)
°°7 8
tagGroupElement
°°8 G
)
°°G H
)
°°H I
;
°°I J
}
¢¢ 
return
•• 
	tagGroups
•• 
;
•• 
}
¶¶ 	
private
´´ 
static
´´ 
TagGroupConfig
´´ %
ParseTagGroup
´´& 3
(
´´3 4

XmlElement
´´4 >

xmlElement
´´? I
)
´´I J
{
¨¨ 	
return
≠≠ 
new
≠≠ 
TagGroupConfig
≠≠ %
{
ÆÆ 
BaseTag
ØØ 
=
ØØ  
GetStringAttribute
ØØ ,
(
ØØ, -

xmlElement
ØØ- 7
,
ØØ7 8
$str
ØØ9 B
)
ØØB C
,
ØØC D
Aliases
∞∞ 
=
∞∞ 
GetAttributeList
∞∞ *
(
∞∞* +

xmlElement
∞∞+ 5
,
∞∞5 6
$str
∞∞7 @
)
∞∞@ A
}
±± 
;
±± 
}
≤≤ 	
private
ªª 
static
ªª 
IDictionary
ªª "
<
ªª" #
string
ªª# )
,
ªª) *
string
ªª+ 1
>
ªª1 2
ParseMetaData
ªª3 @
(
ªª@ A

XmlElement
ªªA K

xmlElement
ªªL V
)
ªªV W
{
ºº 	
var
ææ 
metaData
ææ 
=
ææ 
new
ææ 

Dictionary
ææ )
<
ææ) *
string
ææ* 0
,
ææ0 1
string
ææ2 8
>
ææ8 9
(
ææ9 :
)
ææ: ;
;
ææ; <
var
¡¡ 
metaDataElement
¡¡ 
=
¡¡  !

xmlElement
¡¡" ,
.
¡¡, -
SelectSingleNode
¡¡- =
(
¡¡= >
$str
¡¡> H
)
¡¡H I
;
¡¡I J
if
ƒƒ 
(
ƒƒ 
metaDataElement
ƒƒ 
==
ƒƒ  "
null
ƒƒ# '
)
ƒƒ' (
{
≈≈ 
return
∆∆ 
metaData
∆∆ 
;
∆∆  
}
«« 
foreach
   
(
   
var
   
metaDataEntry
   &
in
  ' )
metaDataElement
  * 9
.
  9 :
SelectNodes
  : E
(
  E F
$str
  F K
)
  K L
)
  L M
{
ÀÀ 
var
ÃÃ 
key
ÃÃ 
=
ÃÃ  
GetStringAttribute
ÃÃ ,
(
ÃÃ, -
(
ÃÃ- .

XmlElement
ÃÃ. 8
)
ÃÃ8 9
metaDataEntry
ÃÃ9 F
,
ÃÃF G
$str
ÃÃH M
)
ÃÃM N
;
ÃÃN O
var
ÕÕ 
value
ÕÕ 
=
ÕÕ  
GetStringAttribute
ÕÕ .
(
ÕÕ. /
(
ÕÕ/ 0

XmlElement
ÕÕ0 :
)
ÕÕ: ;
metaDataEntry
ÕÕ; H
,
ÕÕH I
$str
ÕÕJ Q
)
ÕÕQ R
;
ÕÕR S
metaData
ŒŒ 
[
ŒŒ 
key
ŒŒ 
]
ŒŒ 
=
ŒŒ 
value
ŒŒ  %
;
ŒŒ% &
}
œœ 
return
““ 
metaData
““ 
;
““ 
}
”” 	
private
›› 
static
›› 
ICollection
›› "
<
››" #
string
››# )
>
››) *
GetAttributeList
››+ ;
(
››; <

XmlElement
››< F

xmlElement
››G Q
,
››Q R
string
››S Y
attributeName
››Z g
)
››g h
{
ﬁﬁ 	
ICollection
‡‡ 
<
‡‡ 
string
‡‡ 
>
‡‡ 
items
‡‡  %
=
‡‡& '
null
‡‡( ,
;
‡‡, -
var
„„ 
	attribute
„„ 
=
„„ 

xmlElement
„„ &
.
„„& '

Attributes
„„' 1
.
„„1 2
GetNamedItem
„„2 >
(
„„> ?
attributeName
„„? L
)
„„L M
;
„„M N
if
ÊÊ 
(
ÊÊ 
	attribute
ÊÊ 
!=
ÊÊ 
null
ÊÊ !
)
ÊÊ! "
{
ÁÁ 
var
ËË 
attributeValue
ËË "
=
ËË# $
	attribute
ËË% .
.
ËË. /
Value
ËË/ 4
;
ËË4 5
var
ÈÈ 
attributeArray
ÈÈ "
=
ÈÈ# $
attributeValue
ÈÈ% 3
.
ÈÈ3 4
Split
ÈÈ4 9
(
ÈÈ9 :
$char
ÈÈ: =
)
ÈÈ= >
;
ÈÈ> ?
items
ÍÍ 
=
ÍÍ 
attributeArray
ÍÍ &
.
ÍÍ& '
ToList
ÍÍ' -
(
ÍÍ- .
)
ÍÍ. /
;
ÍÍ/ 0
}
ÎÎ 
return
ÓÓ 
items
ÓÓ 
??
ÓÓ 
new
ÓÓ 
List
ÓÓ  $
<
ÓÓ$ %
string
ÓÓ% +
>
ÓÓ+ ,
(
ÓÓ, -
)
ÓÓ- .
;
ÓÓ. /
}
ÔÔ 	
private
ÙÙ 
static
ÙÙ 
bool
ÙÙ !
GetBooleanAttribute
ÙÙ /
(
ÙÙ/ 0

XmlElement
ÙÙ0 :

xmlElement
ÙÙ; E
,
ÙÙE F
string
ÙÙG M
attributeName
ÙÙN [
,
ÙÙ[ \
bool
ÙÙ] a
defaultValue
ÙÙb n
=
ÙÙo p
false
ÙÙq v
)
ÙÙv w
{
ıı 	
var
˜˜ 
	attribute
˜˜ 
=
˜˜ 

xmlElement
˜˜ &
.
˜˜& '

Attributes
˜˜' 1
.
˜˜1 2
GetNamedItem
˜˜2 >
(
˜˜> ?
attributeName
˜˜? L
)
˜˜L M
;
˜˜M N
if
˙˙ 
(
˙˙ 
	attribute
˙˙ 
==
˙˙ 
null
˙˙ !
)
˙˙! "
{
˚˚ 
return
¸¸ 
defaultValue
¸¸ #
;
¸¸# $
}
˝˝ 
var
ÄÄ 
attributeValue
ÄÄ 
=
ÄÄ  
	attribute
ÄÄ! *
.
ÄÄ* +
Value
ÄÄ+ 0
;
ÄÄ0 1
bool
ÅÅ 
parsed
ÅÅ 
=
ÅÅ 
false
ÅÅ 
;
ÅÅ  
if
ÇÇ 
(
ÇÇ 
bool
ÇÇ 
.
ÇÇ 
TryParse
ÇÇ 
(
ÇÇ 
attributeValue
ÇÇ ,
,
ÇÇ, -
out
ÇÇ. 1
parsed
ÇÇ2 8
)
ÇÇ8 9
)
ÇÇ9 :
{
ÉÉ 
return
ÖÖ 
parsed
ÖÖ 
;
ÖÖ 
}
ÜÜ 
else
áá 
{
àà 
return
ää 
defaultValue
ää #
;
ää# $
}
ãã 
}
åå 	
private
ëë 
static
ëë 
string
ëë  
GetStringAttribute
ëë 0
(
ëë0 1

XmlElement
ëë1 ;

xmlElement
ëë< F
,
ëëF G
string
ëëH N
attributeName
ëëO \
)
ëë\ ]
{
íí 	
var
îî 
	attribute
îî 
=
îî 

xmlElement
îî &
.
îî& '

Attributes
îî' 1
.
îî1 2
GetNamedItem
îî2 >
(
îî> ?
attributeName
îî? L
)
îîL M
;
îîM N
return
óó 
	attribute
óó 
!=
óó 
null
óó  $
?
óó% &
	attribute
óó' 0
.
óó0 1
Value
óó1 6
:
óó7 8
null
óó9 =
;
óó= >
}
òò 	
private
ùù 
static
ùù 
IDictionary
ùù "
<
ùù" #
string
ùù# )
,
ùù) *
object
ùù+ 1
>
ùù1 2'
GetAttributesAsDictionary
ùù3 L
(
ùùL M

XmlElement
ùùM W

xmlElement
ùùX b
)
ùùb c
{
ûû 	
var
†† 
dict
†† 
=
†† 
new
†† 

Dictionary
†† %
<
††% &
string
††& ,
,
††, -
object
††. 4
>
††4 5
(
††5 6
)
††6 7
;
††7 8
foreach
££ 
(
££ 
var
££ 
	attribute
££ "
in
££# %

xmlElement
££& 0
.
££0 1

Attributes
££1 ;
)
££; <
{
§§ 
var
•• 
attr
•• 
=
•• 
(
•• 
XmlAttribute
•• (
)
••( )
	attribute
••) 2
;
••2 3
dict
¶¶ 
[
¶¶ 
attr
¶¶ 
.
¶¶ 
Name
¶¶ 
]
¶¶ 
=
¶¶  !
attr
¶¶" &
.
¶¶& '
Value
¶¶' ,
;
¶¶, -
}
ßß 
return
™™ 
dict
™™ 
;
™™ 
}
´´ 	
}
ÆÆ 
}ØØ Ô
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
} ∞F
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
OrdinalIgnoreCase	\\p Å
)
\\Å Ç
)
\\Ç É
;
\\É Ñ
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
ÄÄ 	
try
ÅÅ 
{
ÇÇ 
this
ÉÉ 
.
ÉÉ 
fallbackLogger
ÉÉ #
.
ÉÉ# $
Log
ÉÉ$ '
(
ÉÉ' (
	exception
ÉÉ( 1
,
ÉÉ1 2
target
ÉÉ3 9
,
ÉÉ9 :
logEvent
ÉÉ; C
)
ÉÉC D
;
ÉÉD E
}
ÑÑ 
catch
ÖÖ 
(
ÖÖ 
	Exception
ÖÖ 
cause
ÖÖ "
)
ÖÖ" #
{
ÜÜ 
Trace
áá 
.
áá 

TraceError
áá  
(
áá  !
$str
áá! o
,
ááo p
cause
ááq v
,
ááv w
	exceptionááx Å
)ááÅ Ç
;ááÇ É
}
àà 
}
ââ 	
public
ãã 
void
ãã  
EnqueueForDispatch
ãã &
(
ãã& '
	ILogEvent
ãã' 0
logEvent
ãã1 9
)
ãã9 :
{
åå 	
if
éé 
(
éé 
isDisposing
éé 
)
éé 
{
èè 
throw
êê 
new
êê '
InvalidOperationException
êê 3
(
êê3 4
$str
êê4 {
)
êê{ |
;
êê| }
}
ëë 
logEventQueue
îî 
.
îî 
Enqueue
îî !
(
îî! "
logEvent
îî" *
)
îî* +
;
îî+ ,
}
ïï 	
public
ôô 
void
ôô 
Dispose
ôô 
(
ôô 
)
ôô 
{
öö 	
Dispose
úú 
(
úú 
true
úú 
)
úú 
;
úú 
GC
üü 
.
üü 
SuppressFinalize
üü 
(
üü  
this
üü  $
)
üü$ %
;
üü% &
}
†† 	
	protected
•• 
virtual
•• 
void
•• 
Dispose
•• &
(
••& '
bool
••' +
	disposing
••, 5
)
••5 6
{
¶¶ 	
if
ßß 
(
ßß 
	disposing
ßß 
)
ßß 
{
®® 
isDisposing
™™ 
=
™™ 
true
™™ "
;
™™" #
this
≠≠ 
.
≠≠  
logEventQueueTimer
≠≠ '
.
≠≠' (
Dispose
≠≠( /
(
≠≠/ 0
)
≠≠0 1
;
≠≠1 2
this
ÆÆ 
.
ÆÆ  
logEventQueueTimer
ÆÆ '
=
ÆÆ( )
null
ÆÆ* .
;
ÆÆ. /
}
ØØ 
ProcessLogQueue
≤≤ 
(
≤≤ 
)
≤≤ 
;
≤≤ 
foreach
µµ 
(
µµ 
var
µµ 
target
µµ 
in
µµ  "
this
µµ# '
.
µµ' (
targets
µµ( /
)
µµ/ 0
{
∂∂ 
target
∑∑ 
.
∑∑ 
Dispose
∑∑ 
(
∑∑ 
)
∑∑  
;
∑∑  !
}
∏∏ 
}
ππ 	
private
¬¬ 
static
¬¬ 
void
¬¬ $
OnLogQueueTimerElapsed
¬¬ 2
(
¬¬2 3
object
¬¬3 9 
dispatcherInstance
¬¬: L
)
¬¬L M
{
√√ 	
var
ƒƒ 

dispatcher
ƒƒ 
=
ƒƒ  
dispatcherInstance
ƒƒ /
as
ƒƒ0 2 
StandardDispatcher
ƒƒ3 E
;
ƒƒE F

dispatcher
≈≈ 
.
≈≈ 
ProcessLogQueue
≈≈ &
(
≈≈& '
)
≈≈' (
;
≈≈( )
}
∆∆ 	
	protected
ÀÀ 
void
ÀÀ 
ProcessLogQueue
ÀÀ &
(
ÀÀ& '
)
ÀÀ' (
{
ÃÃ 	
	ILogEvent
‘‘ 
logEvent
‘‘ 
;
‘‘ 
while
’’ 
(
’’ 
this
’’ 
.
’’ 
logEventQueue
’’ %
.
’’% &

TryDequeue
’’& 0
(
’’0 1
out
’’1 4
logEvent
’’5 =
)
’’= >
)
’’> ?
{
÷÷ 
DispatchNow
◊◊ 
(
◊◊ 
logEvent
◊◊ $
)
◊◊$ %
;
◊◊% &
}
ÿÿ 
}
⁄⁄ 	
}
›› 
}ﬁﬁ ≠
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
} ·	
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
}-- ™
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
} …Æ
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
$str	""2 µ
;
""µ ∂
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
ÄÄ 
(
ÄÄ 
FactoryLock
ÄÄ %
)
ÄÄ% &
{
ÅÅ 
if
ÇÇ 
(
ÇÇ 
_defaultMetaData
ÇÇ ,
==
ÇÇ- /
null
ÇÇ0 4
)
ÇÇ4 5
{
ÉÉ 
_defaultMetaData
áá ,
=
áá- .
new
áá/ 2 
ReadOnlyDictionary
áá3 E
<
ááE F
string
ááF L
,
ááL M
object
ááN T
>
ááT U
(
ááU V

ToMetaData
ááV `
(
áá` a
Config
ááa g
.
áág h
MetaData
ááh p
)
ááp q
)
ááq r
;
áár s
}
ââ 
}
ää 
}
ãã 
return
åå 
_defaultMetaData
åå '
;
åå' (
}
çç 
}
éé 	
private
ìì 
bool
ìì 
isDisposing
ìì  
;
ìì  !
public
ïï #
StandardLoggerFactory
ïï $
(
ïï$ %
Config
ïï% +
config
ïï, 2
)
ïï2 3
{
ññ 	
Config
óó 
=
óó 
config
óó 
;
óó 
try
ûû 
{
üü 
FallbackLogger
†† 
=
††  
GetFallbackLogger
††! 2
(
††2 3
)
††3 4
;
††4 5
}
°° 
catch
¢¢ 
(
¢¢ 
	Exception
¢¢ 
cause
¢¢ "
)
¢¢" #
{
££ 
FallbackLogger
§§ 
=
§§  
new
§§! $)
StandardTraceFallbackLogger
§§% @
(
§§@ A
)
§§A B
;
§§B C
FallbackLogger
•• 
.
•• 
Log
•• "
(
••" #
$str
••# Q
,
••Q R
cause
••S X
)
••X Y
;
••Y Z
}
¶¶ 
}
ßß 	
public
©© 
ILogger
©© 
	GetLogger
©©  
(
©©  !
IMetaDataProvider
©©! 2
metaDataProvider
©©3 C
,
©©C D
IEnumerable
©©E P
<
©©P Q
string
©©Q W
>
©©W X
defaultTags
©©Y d
)
©©d e
{
™™ 	
return
´´ 
new
´´ 
StandardLogger
´´ %
(
´´% &

Dispatcher
´´& 0
,
´´0 1
TagNormalizer
´´2 ?
,
´´? @
metaDataProvider
´´A Q
,
´´Q R
defaultTags
´´S ^
,
´´^ _
DefaultMetaData
´´` o
,
´´o p
Config
´´q w
.
´´w x 
IncludeStackFrame´´x â
)´´â ä
;´´ä ã
}
¨¨ 	
public
ÆÆ 
virtual
ÆÆ 
IDispatcher
ÆÆ "
GetDispatcher
ÆÆ# 0
(
ÆÆ0 1
)
ÆÆ1 2
{
ØØ 	
var
∞∞ 
targets
∞∞ 
=
∞∞ 

GetTargets
∞∞ $
(
∞∞$ %
)
∞∞% &
;
∞∞& '
var
±± 
	tagRouter
±± 
=
±± 
GetTagRouter
±± (
(
±±( )
)
±±) *
;
±±* +
return
≤≤ 
new
≤≤  
StandardDispatcher
≤≤ )
(
≤≤) *
targets
≤≤* 1
,
≤≤1 2
	tagRouter
≤≤3 <
,
≤≤< =
null
≤≤> B
)
≤≤B C
;
≤≤C D
}
≥≥ 	
public
µµ 
virtual
µµ 
ICollection
µµ "
<
µµ" #
ITarget
µµ# *
>
µµ* +

GetTargets
µµ, 6
(
µµ6 7
)
µµ7 8
{
∂∂ 	
var
∑∑ 
targets
∑∑ 
=
∑∑ 
new
∑∑ 
List
∑∑ "
<
∑∑" #
ITarget
∑∑# *
>
∑∑* +
(
∑∑+ ,
)
∑∑, -
;
∑∑- .
if
∫∫ 
(
∫∫ 
Config
∫∫ 
.
∫∫ 
Targets
∫∫ 
==
∫∫ !
null
∫∫" &
)
∫∫& '
{
ªª 
return
ºº 
targets
ºº 
;
ºº 
}
ΩΩ 
foreach
øø 
(
øø 
var
øø 
targetConfig
øø %
in
øø& (
Config
øø) /
.
øø/ 0
Targets
øø0 7
)
øø7 8
{
¿¿ 
var
¡¡ 
target
¡¡ 
=
¡¡ 
BuildTarget
¡¡ (
(
¡¡( )
targetConfig
¡¡) 5
)
¡¡5 6
;
¡¡6 7
if
¬¬ 
(
¬¬ 
target
¬¬ 
!=
¬¬ 
null
¬¬ "
)
¬¬" #
{
√√ 
targets
ƒƒ 
.
ƒƒ 
Add
ƒƒ 
(
ƒƒ  
target
ƒƒ  &
)
ƒƒ& '
;
ƒƒ' (
}
≈≈ 
}
∆∆ 
return
»» 
targets
»» 
;
»» 
}
…… 	
public
ÀÀ 
virtual
ÀÀ  
ITagGroupProcessor
ÀÀ )"
GetTagGroupProcessor
ÀÀ* >
(
ÀÀ> ?
)
ÀÀ? @
{
ÃÃ 	
return
ÕÕ 
new
ÕÕ '
StandardTagGroupProcessor
ÕÕ 0
(
ÕÕ0 1
ToTagGroups
ÕÕ1 <
(
ÕÕ< =
Config
ÕÕ= C
.
ÕÕC D
	TagGroups
ÕÕD M
)
ÕÕM N
)
ÕÕN O
;
ÕÕO P
}
ŒŒ 	
public
–– 
virtual
–– 
IRuleProcessor
–– %
GetRuleProcessor
––& 6
(
––6 7
)
––7 8
{
—— 	
var
““ 
tagGroupProcessor
““ !
=
““" #"
GetTagGroupProcessor
““$ 8
(
““8 9
)
““9 :
;
““: ;
return
”” 
new
”” #
StandardRuleProcessor
”” ,
(
””, -
ToRules
””- 4
(
””4 5
Config
””5 ;
.
””; <
Rules
””< A
)
””A B
,
””B C
tagGroupProcessor
””D U
)
””U V
;
””V W
}
‘‘ 	
public
÷÷ 
virtual
÷÷ 

ITagRouter
÷÷ !
GetTagRouter
÷÷" .
(
÷÷. /
)
÷÷/ 0
{
◊◊ 	
var
ÿÿ 
ruleProcessor
ÿÿ 
=
ÿÿ 
GetRuleProcessor
ÿÿ  0
(
ÿÿ0 1
)
ÿÿ1 2
;
ÿÿ2 3
return
ŸŸ 
new
ŸŸ 
StandardTagRouter
ŸŸ (
(
ŸŸ( )
ruleProcessor
ŸŸ) 6
)
ŸŸ6 7
;
ŸŸ7 8
}
⁄⁄ 	
public
‹‹ 
virtual
‹‹ 
ITagNormalizer
‹‹ %
GetTagNormalizer
‹‹& 6
(
‹‹6 7
)
‹‹7 8
{
›› 	
return
ﬁﬁ 
new
ﬁﬁ #
StandardTagNormalizer
ﬁﬁ ,
(
ﬁﬁ, -
)
ﬁﬁ- .
;
ﬁﬁ. /
}
ﬂﬂ 	
public
·· 
virtual
·· 
ILayoutParser
·· $
GetLayoutParser
··% 4
(
··4 5
)
··5 6
{
‚‚ 	
return
„„ 
new
„„ "
StandardLayoutParser
„„ +
(
„„+ ,
)
„„, -
;
„„- .
}
‰‰ 	
public
ÊÊ 
virtual
ÊÊ 
IPropertyParser
ÊÊ &
GetPropertyParser
ÊÊ' 8
(
ÊÊ8 9
)
ÊÊ9 :
{
ÁÁ 	
return
ËË 
new
ËË $
StandardPropertyParser
ËË -
(
ËË- .
)
ËË. /
;
ËË/ 0
}
ÈÈ 	
public
ÎÎ 
virtual
ÎÎ 
ILayout
ÎÎ 
	GetLayout
ÎÎ (
(
ÎÎ( )
string
ÎÎ) /
format
ÎÎ0 6
)
ÎÎ6 7
{
ÏÏ 	
var
ÓÓ 
layoutParser
ÓÓ 
=
ÓÓ 
GetLayoutParser
ÓÓ .
(
ÓÓ. /
)
ÓÓ/ 0
;
ÓÓ0 1
format
ÔÔ 
=
ÔÔ 
string
ÔÔ 
.
ÔÔ 
IsNullOrEmpty
ÔÔ )
(
ÔÔ) *
format
ÔÔ* 0
)
ÔÔ0 1
?
ÔÔ2 3!
DefaultLayoutFormat
ÔÔ4 G
:
ÔÔH I
format
ÔÔJ P
;
ÔÔP Q
var
 
layoutParms
 
=
 
layoutParser
 *
.
* +
Parse
+ 0
(
0 1
format
1 7
)
7 8
;
8 9
var
ÛÛ 
propertyParser
ÛÛ 
=
ÛÛ  
GetPropertyParser
ÛÛ! 2
(
ÛÛ2 3
)
ÛÛ3 4
;
ÛÛ4 5
return
ˆˆ 
new
ˆˆ 
StandardLayout
ˆˆ %
(
ˆˆ% &
layoutParms
ˆˆ& 1
,
ˆˆ1 2
propertyParser
ˆˆ3 A
)
ˆˆA B
;
ˆˆB C
}
˜˜ 	
public
˘˘ 
virtual
˘˘ 
IFallbackLogger
˘˘ &
GetFallbackLogger
˘˘' 8
(
˘˘8 9
)
˘˘9 :
{
˙˙ 	
if
˚˚ 
(
˚˚ 
Config
˚˚ 
==
˚˚ 
null
˚˚ 
||
˚˚ !
string
˚˚" (
.
˚˚( )
IsNullOrEmpty
˚˚) 6
(
˚˚6 7
Config
˚˚7 =
.
˚˚= >
FallbackLogPath
˚˚> M
)
˚˚M N
)
˚˚N O
{
¸¸ 
return
˝˝ 
new
˝˝ )
StandardTraceFallbackLogger
˝˝ 6
(
˝˝6 7
)
˝˝7 8
;
˝˝8 9
}
˛˛ 
else
ˇˇ 
{
ÄÄ 
return
ÅÅ 
new
ÅÅ (
StandardFileFallbackLogger
ÅÅ 5
(
ÅÅ5 6
Config
ÅÅ6 <
.
ÅÅ< =
FallbackLogPath
ÅÅ= L
)
ÅÅL M
;
ÅÅM N
}
ÇÇ 
}
ÉÉ 	
	protected
ää 
virtual
ää 
IEnumerable
ää %
<
ää% &
TagGroup
ää& .
>
ää. /
ToTagGroups
ää0 ;
(
ää; <
IEnumerable
ää< G
<
ääG H
TagGroupConfig
ääH V
>
ääV W
tagGroupConfigs
ääX g
)
ääg h
{
ãã 	
var
åå 
	tagGroups
åå 
=
åå 
new
åå 
List
åå  $
<
åå$ %
TagGroup
åå% -
>
åå- .
(
åå. /
)
åå/ 0
;
åå0 1
if
èè 
(
èè 
tagGroupConfigs
èè 
==
èè  "
null
èè# '
)
èè' (
{
êê 
return
ëë 
	tagGroups
ëë  
;
ëë  !
}
íí 
foreach
îî 
(
îî 
var
îî 
config
îî 
in
îî  "
tagGroupConfigs
îî# 2
)
îî2 3
{
ïï 
	tagGroups
ññ 
.
ññ 
Add
ññ 
(
ññ 

ToTagGroup
ññ (
(
ññ( )
config
ññ) /
)
ññ/ 0
)
ññ0 1
;
ññ1 2
}
óó 
return
ôô 
	tagGroups
ôô 
;
ôô 
}
öö 	
	protected
üü 
virtual
üü 
TagGroup
üü "

ToTagGroup
üü# -
(
üü- .
TagGroupConfig
üü. <
config
üü= C
)
üüC D
{
†† 	
return
°° 
new
°° 
TagGroup
°° 
{
¢¢ 
BaseTag
££ 
=
££ 
config
££  
.
££  !
BaseTag
££! (
,
££( )
Aliases
§§ 
=
§§ 
config
§§  
.
§§  !
Aliases
§§! (
}
•• 
;
•• 
}
¶¶ 	
	protected
´´ 
virtual
´´ 
IEnumerable
´´ %
<
´´% &
Rule
´´& *
>
´´* +
ToRules
´´, 3
(
´´3 4
IEnumerable
´´4 ?
<
´´? @

RuleConfig
´´@ J
>
´´J K
ruleConfigs
´´L W
)
´´W X
{
¨¨ 	
var
≠≠ 
rules
≠≠ 
=
≠≠ 
new
≠≠ 
List
≠≠  
<
≠≠  !
Rule
≠≠! %
>
≠≠% &
(
≠≠& '
)
≠≠' (
;
≠≠( )
if
∞∞ 
(
∞∞ 
ruleConfigs
∞∞ 
==
∞∞ 
null
∞∞ #
)
∞∞# $
{
±± 
return
≤≤ 
rules
≤≤ 
;
≤≤ 
}
≥≥ 
foreach
µµ 
(
µµ 
var
µµ 
config
µµ 
in
µµ  "
ruleConfigs
µµ# .
)
µµ. /
{
∂∂ 
rules
∑∑ 
.
∑∑ 
Add
∑∑ 
(
∑∑ 
ToRule
∑∑  
(
∑∑  !
config
∑∑! '
)
∑∑' (
)
∑∑( )
;
∑∑) *
}
∏∏ 
return
∫∫ 
rules
∫∫ 
;
∫∫ 
}
ªª 	
	protected
¿¿ 
virtual
¿¿ 
Rule
¿¿ 
ToRule
¿¿ %
(
¿¿% &

RuleConfig
¿¿& 0
config
¿¿1 7
)
¿¿7 8
{
¡¡ 	
return
¬¬ 
new
¬¬ 
Rule
¬¬ 
{
√√ 
Include
ƒƒ 
=
ƒƒ 
config
ƒƒ  
.
ƒƒ  !
Includes
ƒƒ! )
,
ƒƒ) *
StrictInclude
≈≈ 
=
≈≈ 
config
≈≈  &
.
≈≈& '
StrictInclude
≈≈' 4
,
≈≈4 5
Exclude
∆∆ 
=
∆∆ 
config
∆∆  
.
∆∆  !
Excludes
∆∆! )
,
∆∆) *
Targets
«« 
=
«« 
config
««  
.
««  !
Targets
««! (
,
««( )
Final
»» 
=
»» 
config
»» 
.
»» 
Final
»» $
}
…… 
;
…… 
}
   	
	protected
œœ 
virtual
œœ 
IDictionary
œœ %
<
œœ% &
string
œœ& ,
,
œœ, -
object
œœ. 4
>
œœ4 5

ToMetaData
œœ6 @
(
œœ@ A
IDictionary
œœA L
<
œœL M
string
œœM S
,
œœS T
string
œœU [
>
œœ[ \
configMetaData
œœ] k
)
œœk l
{
–– 	
var
—— 
metaData
—— 
=
—— 
new
—— 

Dictionary
—— )
<
——) *
string
——* 0
,
——0 1
object
——2 8
>
——8 9
(
——9 :
)
——: ;
;
——; <
if
‘‘ 
(
‘‘ 
configMetaData
‘‘ 
==
‘‘ !
null
‘‘" &
)
‘‘& '
{
’’ 
return
÷÷ 
metaData
÷÷ 
;
÷÷  
}
◊◊ 
foreach
ŸŸ 
(
ŸŸ 
var
ŸŸ 
entry
ŸŸ 
in
ŸŸ !
configMetaData
ŸŸ" 0
)
ŸŸ0 1
{
⁄⁄ 
metaData
€€ 
[
€€ 
entry
€€ 
.
€€ 
Key
€€ "
]
€€" #
=
€€$ %
entry
€€& +
.
€€+ ,
Value
€€, 1
;
€€1 2
}
‹‹ 
return
ﬁﬁ 
metaData
ﬁﬁ 
;
ﬁﬁ 
}
ﬂﬂ 	
public
ÂÂ 
void
ÂÂ 
Dispose
ÂÂ 
(
ÂÂ 
)
ÂÂ 
{
ÊÊ 	
Dispose
ËË 
(
ËË 
true
ËË 
)
ËË 
;
ËË 
GC
ÎÎ 
.
ÎÎ 
SuppressFinalize
ÎÎ 
(
ÎÎ  
this
ÎÎ  $
)
ÎÎ$ %
;
ÎÎ% &
}
ÏÏ 	
	protected
ÒÒ 
virtual
ÒÒ 
void
ÒÒ 
Dispose
ÒÒ &
(
ÒÒ& '
bool
ÒÒ' +
	disposing
ÒÒ, 5
)
ÒÒ5 6
{
ÚÚ 	
if
ÛÛ 
(
ÛÛ 
	disposing
ÛÛ 
)
ÛÛ 
{
ÙÙ 
isDisposing
ˆˆ 
=
ˆˆ 
true
ˆˆ "
;
ˆˆ" #
}
˜˜ 
if
˘˘ 
(
˘˘ 
_dispatcher
˘˘ 
!=
˘˘ 
null
˘˘ #
)
˘˘# $
{
˙˙ 
_dispatcher
˚˚ 
.
˚˚ 
Dispose
˚˚ #
(
˚˚# $
)
˚˚$ %
;
˚˚% &
_dispatcher
¸¸ 
=
¸¸ 
null
¸¸ "
;
¸¸" #
}
˝˝ 
}
˛˛ 	
~
ÄÄ 	#
StandardLoggerFactory
ÄÄ	 
(
ÄÄ 
)
ÄÄ  
{
ÅÅ 	
Dispose
ÇÇ 
(
ÇÇ 
false
ÇÇ 
)
ÇÇ 
;
ÇÇ 
}
ÉÉ 	
	protected
ââ 
virtual
ââ 
ITarget
ââ !
BuildTarget
ââ" -
(
ââ- .
TargetConfig
ââ. :
targetConfig
ââ; G
)
ââG H
{
ää 	
try
ãã 
{
åå 
var
éé 
type
éé 
=
éé 
Type
éé 
.
éé  
GetType
éé  '
(
éé' (
targetConfig
éé( 4
.
éé4 5
Type
éé5 9
)
éé9 :
;
éé: ;
if
êê 
(
êê 
type
êê 
!=
êê 
null
êê  
)
êê  !
{
ëë 
var
íí 
target
íí 
=
íí  
(
íí! "
ITarget
íí" )
)
íí) *
	Activator
íí* 3
.
íí3 4
CreateInstance
íí4 B
(
ííB C
type
ííC G
)
ííG H
;
ííH I
target
ïï 
.
ïï 
	Configure
ïï $
(
ïï$ %
targetConfig
ïï% 1
)
ïï1 2
;
ïï2 3
target
ññ 
.
ññ 
Name
ññ 
=
ññ  !
targetConfig
ññ" .
.
ññ. /
Name
ññ/ 3
;
ññ3 4
if
ôô 
(
ôô 
ILayoutTargetType
ôô )
.
ôô) *
IsAssignableFrom
ôô* :
(
ôô: ;
target
ôô; A
.
ôôA B
GetType
ôôB I
(
ôôI J
)
ôôJ K
)
ôôK L
)
ôôL M
{
öö 
var
õõ 
layoutTarget
õõ (
=
õõ) *
(
õõ+ ,
ILayoutTarget
õõ, 9
)
õõ9 :
target
õõ: @
;
õõ@ A
layoutTarget
úú $
.
úú$ %
	Configure
úú% .
(
úú. /
targetConfig
úú/ ;
,
úú; <
this
úú= A
)
úúA B
;
úúB C
}
ùù 
return
†† 
target
†† !
;
††! "
}
°° 
else
¢¢ 
{
££ 
FallbackLogger
§§ "
.
§§" #
Log
§§# &
(
§§& '
$str§§' ã
,§§ã å
targetConfig§§ç ô
.§§ô ö
Name§§ö û
,§§û ü
targetConfig§§† ¨
.§§¨ ≠
Type§§≠ ±
)§§± ≤
;§§≤ ≥
return
•• 
null
•• 
;
••  
}
¶¶ 
}
ßß 
catch
®® 
(
®® 
	Exception
®® 
cause
®® "
)
®®" #
{
©© 
FallbackLogger
™™ 
.
™™ 
Log
™™ "
(
™™" #
$str
™™# w
,
™™w x
targetConfig
´´  
!=
´´! #
null
´´$ (
?
´´) *
targetConfig
´´+ 7
.
´´7 8
Name
´´8 <
:
´´= >
string
´´? E
.
´´E F
Empty
´´F K
,
´´K L
targetConfig
¨¨  
!=
¨¨! #
null
¨¨$ (
?
¨¨) *
targetConfig
¨¨+ 7
.
¨¨7 8
Type
¨¨8 <
:
¨¨= >
string
¨¨? E
.
¨¨E F
Empty
¨¨F K
,
¨¨K L
cause
≠≠ 
)
≠≠ 
;
≠≠ 
return
ÆÆ 
null
ÆÆ 
;
ÆÆ 
}
ØØ 
}
∞∞ 	
}
≥≥ 
}¥¥ ¬
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
}%% ü'
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
GetType	~ Ö
(
Ö Ü
)
Ü á
.
á à
FullName
à ê
,
ê ë
JoinTags
í ö
(
ö õ
logEvent
õ £
)
£ §
,
§ •

GetMessage
¶ ∞
(
∞ ±
logEvent
± π
)
π ∫
,
∫ ª!
GetExceptionMessage
º œ
(
œ –
logEvent
– ÿ
)
ÿ Ÿ
,
Ÿ ⁄
	exception
€ ‰
)
‰ Â
;
Â Ê
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
};; ˇ
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
} Ω
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
} ò
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
} ¥
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
}<< «
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
} ≠
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
} ﬂ
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
} ì
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
} Î
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
} Õ
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
} ì
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
}(( ÔC
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
}rr ˛=
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
ÄÄ 
(
ÄÄ 
dictionaryTypes
ÄÄ 
.
ÄÄ  
ContainsKey
ÄÄ  +
(
ÄÄ+ ,

objectType
ÄÄ, 6
)
ÄÄ6 7
)
ÄÄ7 8
{
ÅÅ 
return
ÇÇ 
dictionaryTypes
ÇÇ &
[
ÇÇ& '

objectType
ÇÇ' 1
]
ÇÇ1 2
;
ÇÇ2 3
}
ÉÉ 
var
ÖÖ 
isAssignable
ÖÖ 
=
ÖÖ 
DictionaryType
ÖÖ -
.
ÖÖ- .
IsAssignableFrom
ÖÖ. >
(
ÖÖ> ?

objectType
ÖÖ? I
)
ÖÖI J
;
ÖÖJ K
dictionaryTypes
ÜÜ 
[
ÜÜ 

objectType
ÜÜ &
]
ÜÜ& '
=
ÜÜ( )
isAssignable
ÜÜ* 6
;
ÜÜ6 7
return
áá 
isAssignable
áá 
;
áá  
}
àà 	
}
ââ 
}ää ãh
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
ÉÉ 
virtual
ÉÉ 
object
ÉÉ  !
GetSpecialParameter
ÉÉ! 4
(
ÉÉ4 5
LogEvent
ÉÉ5 =
logEvent
ÉÉ> F
,
ÉÉF G
LayoutParameter
ÉÉH W
	parameter
ÉÉX a
)
ÉÉa b
{
ÑÑ 	
switch
ÖÖ 
(
ÖÖ 
	parameter
ÖÖ 
.
ÖÖ 
Path
ÖÖ "
)
ÖÖ" #
{
ÜÜ 
case
áá 
$str
áá 
:
áá 
return
ãã 
logEvent
ãã #
.
ãã# $
Tags
ãã$ (
!=
ãã) +
null
ãã, 0
?
ãã1 2
string
ãã3 9
.
ãã9 :
Join
ãã: >
(
ãã> ?
$str
ãã? B
,
ããB C
logEvent
ããD L
.
ããL M
Tags
ããM Q
)
ããQ R
:
ããS T
string
ããU [
.
ãã[ \
Empty
ãã\ a
;
ããa b
case
éé 
$str
éé  
:
éé  !
return
èè 
FormatException
èè *
(
èè* +
logEvent
èè+ 3
.
èè3 4
	Exception
èè4 =
)
èè= >
;
èè> ?
default
ëë 
:
ëë 
return
íí 
null
íí 
;
íí  
}
ìì 
}
îî 	
private
ôô 
static
ôô 
string
ôô 
FormatException
ôô -
(
ôô- .
	Exception
ôô. 7
	exception
ôô8 A
)
ôôA B
{
öö 	
var
õõ 
sb
õõ 
=
õõ 
new
õõ 
StringBuilder
õõ &
(
õõ& '
)
õõ' (
;
õõ( )
var
ùù 
depth
ùù 
=
ùù 
$num
ùù 
;
ùù 
bool
ûû 
inner
ûû 
=
ûû 
false
ûû 
;
ûû 
while
üü 
(
üü 
	exception
üü 
!=
üü 
null
üü  $
&&
üü% '
depth
üü( -
++
üü- /
<
üü0 1
$num
üü2 4
)
üü4 5
{
†† 
sb
°° 
.
°° 
Append
°° 
(
°° 
string
°°  
.
°°  !
Format
°°! '
(
°°' (
$str
°°( 9
,
°°9 :
inner
°°; @
?
°°A B
$str
°°C O
:
°°P Q
$str
°°R T
,
°°T U
	exception
°°V _
.
°°_ `
GetType
°°` g
(
°°g h
)
°°h i
.
°°i j
FullName
°°j r
,
°°r s
	exception
°°t }
.
°°} ~
Message°°~ Ö
)°°Ö Ü
)°°Ü á
;°°á à
sb
¢¢ 
.
¢¢ 
Append
¢¢ 
(
¢¢ 
string
¢¢  
.
¢¢  !
Format
¢¢! '
(
¢¢' (
$str
¢¢( 1
,
¢¢1 2
	exception
¢¢3 <
.
¢¢< =

StackTrace
¢¢= G
)
¢¢G H
)
¢¢H I
;
¢¢I J
	exception
££ 
=
££ 
	exception
££ %
.
££% &
InnerException
££& 4
;
££4 5
inner
§§ 
=
§§ 
true
§§ 
;
§§ 
}
•• 
return
ßß 
sb
ßß 
.
ßß 
ToString
ßß 
(
ßß 
)
ßß  
;
ßß  !
}
®® 	
private
≠≠ 
static
≠≠ 
bool
≠≠ !
IsNullOrEmptyString
≠≠ /
(
≠≠/ 0
object
≠≠0 6
value
≠≠7 <
)
≠≠< =
{
ÆÆ 	
return
ØØ 
value
ØØ 
==
ØØ 
null
ØØ  
||
∞∞ 
(
∞∞ 
typeof
∞∞ 
(
∞∞ 
string
∞∞ !
)
∞∞! "
.
∞∞" #
IsAssignableFrom
∞∞# 3
(
∞∞3 4
value
∞∞4 9
.
∞∞9 :
GetType
∞∞: A
(
∞∞A B
)
∞∞B C
)
∞∞C D
&&
∞∞E G
string
∞∞H N
.
∞∞N O
IsNullOrEmpty
∞∞O \
(
∞∞\ ]
(
∞∞] ^
string
∞∞^ d
)
∞∞d e
value
∞∞e j
)
∞∞j k
)
∞∞k l
;
∞∞l m
}
±± 	
private
∂∂ 
static
∂∂ 
string
∂∂ 
GetFormattedValue
∂∂ /
(
∂∂/ 0
object
∂∂0 6
value
∂∂7 <
,
∂∂< =
string
∂∂> D
format
∂∂E K
)
∂∂K L
{
∑∑ 	
if
ππ 
(
ππ 
value
ππ 
==
ππ 
null
ππ 
)
ππ 
{
∫∫ 
return
ªª 
string
ªª 
.
ªª 
Empty
ªª #
;
ªª# $
}
ºº 
if
øø 
(
øø 
string
øø 
.
øø 
IsNullOrEmpty
øø $
(
øø$ %
format
øø% +
)
øø+ ,
==
øø- /
false
øø0 5
)
øø5 6
{
¿¿ 
return
¡¡ 
string
¡¡ 
.
¡¡ 
Format
¡¡ $
(
¡¡$ %
format
¡¡% +
,
¡¡+ ,
value
¡¡- 2
)
¡¡2 3
;
¡¡3 4
}
¬¬ 
if
≈≈ 
(
≈≈ 
value
≈≈ 
is
≈≈ 
string
≈≈ 
)
≈≈  
{
∆∆ 
return
«« 
(
«« 
string
«« 
)
«« 
value
«« $
;
««$ %
}
»» 
else
…… 
if
…… 
(
…… 
iEnumerableType
…… $
.
……$ %
IsAssignableFrom
……% 5
(
……5 6
value
……6 ;
.
……; <
GetType
……< C
(
……C D
)
……D E
)
……E F
==
……G I
false
……J O
)
……O P
{
   
return
ÃÃ 
Convert
ÃÃ 
.
ÃÃ 
ToString
ÃÃ '
(
ÃÃ' (
value
ÃÃ( -
)
ÃÃ- .
;
ÃÃ. /
}
ÕÕ 
else
ŒŒ 
{
œœ 
return
—— 
string
—— 
.
—— 
Format
—— $
(
——$ %
$str
——% ,
,
——, -
string
——. 4
.
——4 5
Join
——5 9
(
——9 :
$str
——: =
,
——= >
EnumerateValues
——? N
(
——N O
(
——O P
IEnumerable
——P [
)
——[ \
value
——\ a
)
——a b
)
——b c
)
——c d
;
——d e
}
““ 
}
”” 	
private
ÿÿ 
static
ÿÿ 
string
ÿÿ 
[
ÿÿ 
]
ÿÿ 
EnumerateValues
ÿÿ  /
(
ÿÿ/ 0
IEnumerable
ÿÿ0 ;
items
ÿÿ< A
)
ÿÿA B
{
ŸŸ 	
var
⁄⁄ 
list
⁄⁄ 
=
⁄⁄ 
new
⁄⁄ 
List
⁄⁄ 
<
⁄⁄  
string
⁄⁄  &
>
⁄⁄& '
(
⁄⁄' (
)
⁄⁄( )
;
⁄⁄) *
foreach
‹‹ 
(
‹‹ 
var
‹‹ 
item
‹‹ 
in
‹‹  
items
‹‹! &
)
‹‹& '
{
›› 
list
ﬁﬁ 
.
ﬁﬁ 
Add
ﬁﬁ 
(
ﬁﬁ 
Convert
ﬁﬁ  
.
ﬁﬁ  !
ToString
ﬁﬁ! )
(
ﬁﬁ) *
item
ﬁﬁ* .
)
ﬁﬁ. /
)
ﬁﬁ/ 0
;
ﬁﬁ0 1
}
ﬂﬂ 
return
·· 
list
·· 
.
·· 
ToArray
·· 
(
··  
)
··  !
;
··! "
}
‚‚ 	
}
„„ 
}‰‰ ∆
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
}HH Ω
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
}   ∆
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
} ÌÅ
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
IEnumerable	99x É
<
99É Ñ
string
99Ñ ä
>
99ä ã
defaultTags
99å ó
=
99ò ô
null
99ö û
,
99û ü
IDictionary
99† ´
<
99´ ¨
string
99¨ ≤
,
99≤ ≥
object
99¥ ∫
>
99∫ ª
defaultMetaData
99º À
=
99Ã Õ
null
99Œ “
,
99“ ”
bool
99‘ ÿ
includeStackFrame
99Ÿ Í
=
99Î Ï
false
99Ì Ú
)
99Ú Û
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
tags	kk| Ä
)
kkÄ Å
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
string	ss{ Å
[
ssÅ Ç
]
ssÇ É
tags
ssÑ à
)
ssà â
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
ÑÑ 
virtual
ÑÑ 
ICollection
ÑÑ %
<
ÑÑ% &
string
ÑÑ& ,
>
ÑÑ, -
GetTags
ÑÑ. 5
(
ÑÑ5 6
ICollection
ÑÑ6 A
<
ÑÑA B
string
ÑÑB H
>
ÑÑH I
	givenTags
ÑÑJ S
,
ÑÑS T
bool
ÑÑU Y
hasException
ÑÑZ f
)
ÑÑf g
{
ÖÖ 	
ICollection
áá 
<
áá 
string
áá 
>
áá 
tags
áá  $
;
áá$ %
if
ãã 
(
ãã 
!
ãã 
HasTags
ãã 
(
ãã 
	givenTags
ãã "
)
ãã" #
&&
ãã$ &
!
ãã' (
HasTags
ãã( /
(
ãã/ 0
DefaultTags
ãã0 ;
)
ãã; <
)
ãã< =
{
åå 
tags
çç 
=
çç 
new
çç 
List
çç 
<
çç  
string
çç  &
>
çç& '
(
çç' (
)
çç( )
;
çç) *
}
éé 
else
èè 
if
èè 
(
èè 
!
èè 
HasTags
èè 
(
èè 
DefaultTags
èè )
)
èè) *
)
èè* +
{
êê 
tags
ëë 
=
ëë 
TagNormalizer
ëë $
.
ëë$ %
NormalizeTags
ëë% 2
(
ëë2 3
	givenTags
ëë3 <
)
ëë< =
;
ëë= >
}
íí 
else
ìì 
if
ìì 
(
ìì 
!
ìì 
HasTags
ìì 
(
ìì 
	givenTags
ìì '
)
ìì' (
)
ìì( )
{
îî 
tags
ïï 
=
ïï 
new
ïï 
List
ïï 
<
ïï  
string
ïï  &
>
ïï& '
(
ïï' (
)
ïï( )
;
ïï) *
foreach
ññ 
(
ññ 
var
ññ 
tag
ññ  
in
ññ! #
DefaultTags
ññ$ /
)
ññ/ 0
{
óó 
tags
òò 
.
òò 
Add
òò 
(
òò 
tag
òò  
)
òò  !
;
òò! "
}
ôô 
}
öö 
else
õõ 
{
úú 
tags
ùù 
=
ùù 
new
ùù 
List
ùù 
<
ùù  
string
ùù  &
>
ùù& '
(
ùù' (
)
ùù( )
;
ùù) *
foreach
ûû 
(
ûû 
var
ûû 
tag
ûû  
in
ûû! #
DefaultTags
ûû$ /
)
ûû/ 0
{
üü 
tags
†† 
.
†† 
Add
†† 
(
†† 
tag
††  
)
††  !
;
††! "
}
°° 
foreach
¢¢ 
(
¢¢ 
var
¢¢ 
tag
¢¢  
in
¢¢! #
	givenTags
¢¢$ -
)
¢¢- .
{
££ 
tags
§§ 
.
§§ 
Add
§§ 
(
§§ 
tag
§§  
)
§§  !
;
§§! "
}
•• 
tags
¶¶ 
=
¶¶ 
TagNormalizer
¶¶ $
.
¶¶$ %
NormalizeTags
¶¶% 2
(
¶¶2 3
tags
¶¶3 7
)
¶¶7 8
;
¶¶8 9
}
ßß 
if
™™ 
(
™™ 
hasException
™™ 
)
™™ 
{
´´ 
tags
¨¨ 
.
¨¨ 
Add
¨¨ 
(
¨¨ 
exceptionTag
¨¨ %
)
¨¨% &
;
¨¨& '
}
≠≠ 
return
∞∞ 
tags
∞∞ 
;
∞∞ 
}
±± 	
	protected
∂∂ 
static
∂∂ 
bool
∂∂ 
HasTags
∂∂ %
(
∂∂% &
IEnumerable
∂∂& 1
<
∂∂1 2
string
∂∂2 8
>
∂∂8 9
tags
∂∂: >
)
∂∂> ?
{
∑∑ 	
return
∏∏ 
tags
∏∏ 
!=
∏∏ 
null
∏∏ 
&&
∏∏  "
tags
∏∏# '
.
∏∏' (
Count
∏∏( -
(
∏∏- .
)
∏∏. /
>
∏∏0 1
$num
∏∏2 3
;
∏∏3 4
}
ππ 	
	protected
¡¡ 
IDictionary
¡¡ 
<
¡¡ 
string
¡¡ $
,
¡¡$ %
object
¡¡& ,
>
¡¡, -
GetMetaData
¡¡. 9
(
¡¡9 :
IDictionary
¡¡: E
<
¡¡E F
string
¡¡F L
,
¡¡L M
object
¡¡N T
>
¡¡T U
givenMetaData
¡¡V c
)
¡¡c d
{
¬¬ 	
var
ƒƒ 
metaData
ƒƒ 
=
ƒƒ 
new
ƒƒ 

Dictionary
ƒƒ )
<
ƒƒ) *
string
ƒƒ* 0
,
ƒƒ0 1
object
ƒƒ2 8
>
ƒƒ8 9
(
ƒƒ9 :
)
ƒƒ: ;
;
ƒƒ; <
AddMetaData
«« 
(
«« 
DefaultMetaData
«« '
,
««' (
metaData
««) 1
)
««1 2
;
««2 3
var
   
providedMetaData
    
=
  ! "
MetaDataProvider
  # 3
!=
  4 6
null
  7 ;
?
ÀÀ 
MetaDataProvider
ÀÀ "
.
ÀÀ" #
ProvideMetaData
ÀÀ# 2
(
ÀÀ2 3
)
ÀÀ3 4
:
ÃÃ 
null
ÃÃ 
;
ÃÃ 
AddMetaData
œœ 
(
œœ 
providedMetaData
œœ (
,
œœ( )
metaData
œœ* 2
)
œœ2 3
;
œœ3 4
AddMetaData
““ 
(
““ 
givenMetaData
““ %
,
““% &
metaData
““' /
)
““/ 0
;
““0 1
return
’’ 
metaData
’’ 
;
’’ 
}
÷÷ 	
	protected
€€ 
static
€€ 
void
€€ 
AddMetaData
€€ )
(
€€) *
IDictionary
€€* 5
<
€€5 6
string
€€6 <
,
€€< =
object
€€> D
>
€€D E
sourceMetaData
€€F T
,
€€T U
IDictionary
€€V a
<
€€a b
string
€€b h
,
€€h i
object
€€j p
>
€€p q
targetMetaData€€r Ä
)€€Ä Å
{
‹‹ 	
if
›› 
(
›› 
sourceMetaData
›› 
!=
›› !
null
››" &
)
››& '
{
ﬁﬁ 
foreach
ﬂﬂ 
(
ﬂﬂ 
var
ﬂﬂ 
item
ﬂﬂ !
in
ﬂﬂ" $
sourceMetaData
ﬂﬂ% 3
)
ﬂﬂ3 4
{
‡‡ 
targetMetaData
·· "
[
··" #
item
··# '
.
··' (
Key
··( +
]
··+ ,
=
··- .
item
··/ 3
.
··3 4
Value
··4 9
;
··9 :
}
‚‚ 
}
„„ 
}
‰‰ 	
	protected
ÈÈ 
static
ÈÈ 
bool
ÈÈ 
HasMetaData
ÈÈ )
(
ÈÈ) *
IDictionary
ÈÈ* 5
<
ÈÈ5 6
string
ÈÈ6 <
,
ÈÈ< =
object
ÈÈ> D
>
ÈÈD E
metaData
ÈÈF N
)
ÈÈN O
{
ÍÍ 	
return
ÎÎ 
metaData
ÎÎ 
!=
ÎÎ 
null
ÎÎ #
&&
ÎÎ$ &
metaData
ÎÎ' /
.
ÎÎ/ 0
Count
ÎÎ0 5
>
ÎÎ6 7
$num
ÎÎ8 9
;
ÎÎ9 :
}
ÏÏ 	
}
ÌÌ 
}ÓÓ ä(
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
}rr Ú
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
]$$) *É
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
CultureInvariant	y â
)
â ä
;
ä ã
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
}RR å
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
} ÷
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
} ΩH
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
ÅÅ 
bool
ÅÅ 
IsExcludeMatch
ÅÅ #
(
ÅÅ# $
Rule
ÅÅ$ (
rule
ÅÅ) -
,
ÅÅ- .
IEnumerable
ÅÅ/ :
<
ÅÅ: ;
string
ÅÅ; A
>
ÅÅA B
tags
ÅÅC G
)
ÅÅG H
{
ÇÇ 	
if
ÑÑ 
(
ÑÑ 
rule
ÑÑ 
.
ÑÑ 
Exclude
ÑÑ 
==
ÑÑ 
null
ÑÑ  $
||
ÑÑ% '
rule
ÑÑ( ,
.
ÑÑ, -
Exclude
ÑÑ- 4
.
ÑÑ4 5
Count
ÑÑ5 :
(
ÑÑ: ;
)
ÑÑ; <
==
ÑÑ= ?
$num
ÑÑ@ A
)
ÑÑA B
{
ÖÖ 
return
ÜÜ 
false
ÜÜ 
;
ÜÜ 
}
áá 
foreach
ää 
(
ää 
var
ää 
ruleTag
ää  
in
ää! #
rule
ää$ (
.
ää( )
Exclude
ää) 0
)
ää0 1
{
ãã 
foreach
åå 
(
åå 
var
åå 
tag
åå  
in
åå! #
tags
åå$ (
)
åå( )
{
çç 
if
éé 
(
éé 

IsTagMatch
éé "
(
éé" #
ruleTag
éé# *
,
éé* +
tag
éé, /
)
éé/ 0
)
éé0 1
{
èè 
return
ëë 
true
ëë #
;
ëë# $
}
íí 
}
ìì 
}
îî 
return
óó 
false
óó 
;
óó 
}
òò 	
private
°° 
bool
°° 

IsTagMatch
°° 
(
°°  
string
°°  &
ruleTag
°°' .
,
°°. /
string
°°0 6
tag
°°7 :
)
°°: ;
{
¢¢ 	
var
§§ 
ruleTagPattern
§§ 
=
§§  
GetRuleTagPattern
§§! 2
(
§§2 3
ruleTag
§§3 :
)
§§: ;
;
§§; <
var
ßß 
anyMatch
ßß 
=
ßß 
false
ßß  
;
ßß  !
foreach
®® 
(
®® 
var
®® 
alias
®® 
in
®® !
tagGroupProcessor
®®" 3
.
®®3 4

GetAliases
®®4 >
(
®®> ?
tag
®®? B
)
®®B C
)
®®C D
{
©© 
if
´´ 
(
´´ 
ruleTagPattern
´´ "
.
´´" #
IsMatch
´´# *
(
´´* +
alias
´´+ 0
)
´´0 1
)
´´1 2
{
¨¨ 
anyMatch
≠≠ 
=
≠≠ 
true
≠≠ #
;
≠≠# $
break
ÆÆ 
;
ÆÆ 
}
ØØ 
}
∞∞ 
return
≥≥ 
anyMatch
≥≥ 
;
≥≥ 
}
¥¥ 	
private
ππ 
Regex
ππ 
GetRuleTagPattern
ππ '
(
ππ' (
string
ππ( .
ruleTag
ππ/ 6
)
ππ6 7
{
∫∫ 	
if
ªª 
(
ªª 
ruleTagPatterns
ªª 
.
ªª  
ContainsKey
ªª  +
(
ªª+ ,
ruleTag
ªª, 3
)
ªª3 4
==
ªª5 7
false
ªª8 =
)
ªª= >
{
ºº 
var
ææ 
regexRuleTag
ææ  
=
ææ! "
ruleTag
ææ# *
.
ææ* +
Replace
ææ+ 2
(
ææ2 3
$str
ææ3 6
,
ææ6 7
$str
ææ8 =
)
ææ= >
;
ææ> ?
regexRuleTag
¡¡ 
=
¡¡ 
ruleTag
¡¡ &
.
¡¡& '
Replace
¡¡' .
(
¡¡. /
$str
¡¡/ 2
,
¡¡2 3
$str
¡¡4 8
)
¡¡8 9
;
¡¡9 :
var
ƒƒ 
ruleTagPattern
ƒƒ "
=
ƒƒ# $
new
ƒƒ% (
Regex
ƒƒ) .
(
ƒƒ. /
regexRuleTag
ƒƒ/ ;
,
ƒƒ; <
RegexOptions
ƒƒ= I
.
ƒƒI J
CultureInvariant
ƒƒJ Z
|
ƒƒ[ \
RegexOptions
ƒƒ] i
.
ƒƒi j

IgnoreCase
ƒƒj t
|
ƒƒu v
RegexOptionsƒƒw É
.ƒƒÉ Ñ
CompiledƒƒÑ å
)ƒƒå ç
;ƒƒç é
ruleTagPatterns
«« 
[
««  
ruleTag
««  '
]
««' (
=
««) *
ruleTagPattern
««+ 9
;
««9 :
}
»» 
return
ÀÀ 
ruleTagPatterns
ÀÀ "
[
ÀÀ" #
ruleTag
ÀÀ# *
]
ÀÀ* +
;
ÀÀ+ ,
}
ÃÃ 	
}
œœ 
}–– Ù
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
}@@ ¸
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
}<< Å 
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
}KK î
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
} ˇ
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
} Á

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
$str	8 Ë
,
Ë È
cause
Í Ô
)
Ô 
;
 Ò
} 
throw   
;   
}!! 
}"" 	
}## 
}$$ ´
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
} ¢#
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
}[[ ø
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
} Ü
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
} ∏
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
}77 ¯
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
} Äx
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
$str	7 ∏
;
∏ π
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
ÄÄ "
convertNewlineInHtml
ÅÅ $
=
ÅÅ% &%
convertNewlinInHtmlFlag
ÅÅ' >
;
ÅÅ> ?
}
ÇÇ 
var
ÖÖ 
recipientsString
ÖÖ  
=
ÖÖ! "
GetProperty
ÖÖ# .
<
ÖÖ. /
string
ÖÖ/ 5
>
ÖÖ5 6
(
ÖÖ6 7
config
ÖÖ7 =
,
ÖÖ= >
$str
ÖÖ? C
)
ÖÖC D
;
ÖÖD E
if
ÜÜ 
(
ÜÜ 
string
ÜÜ 
.
ÜÜ 
IsNullOrEmpty
ÜÜ $
(
ÜÜ$ %
recipientsString
ÜÜ% 5
)
ÜÜ5 6
==
ÜÜ7 9
false
ÜÜ: ?
)
ÜÜ? @
{
áá 
to
àà 
=
àà 
recipientsString
àà %
.
àà% &
Split
àà& +
(
àà+ ,
$char
àà, /
)
àà/ 0
;
àà0 1
}
ââ 
var
åå 

fromString
åå 
=
åå 
GetProperty
åå (
<
åå( )
string
åå) /
>
åå/ 0
(
åå0 1
config
åå1 7
,
åå7 8
$str
åå9 ?
)
åå? @
;
åå@ A
if
çç 
(
çç 
string
çç 
.
çç 
IsNullOrEmpty
çç $
(
çç$ %

fromString
çç% /
)
çç/ 0
==
çç1 3
false
çç4 9
)
çç9 :
{
éé 
from
èè 
=
èè 
new
èè 
MailAddress
èè &
(
èè& '

fromString
èè' 1
)
èè1 2
;
èè2 3
}
êê 
var
ìì 
userNameString
ìì 
=
ìì  
GetProperty
ìì! ,
<
ìì, -
string
ìì- 3
>
ìì3 4
(
ìì4 5
config
ìì5 ;
,
ìì; <
$str
ìì= K
)
ììK L
;
ììL M
var
îî 
password
îî 
=
îî 
GetProperty
îî &
<
îî& '
string
îî' -
>
îî- .
(
îî. /
config
îî/ 5
,
îî5 6
$str
îî7 E
)
îîE F
;
îîF G

smtpClient
ïï 
.
ïï 
SetCredentials
ïï %
(
ïï% &
userNameString
ïï& 4
,
ïï4 5
password
ïï6 >
)
ïï> ?
;
ïï? @
var
òò 
enableSslFlagRaw
òò  
=
òò! "
GetProperty
òò# .
<
òò. /
string
òò/ 5
>
òò5 6
(
òò6 7
config
òò7 =
,
òò= >
$str
òò? J
)
òòJ K
;
òòK L
bool
ôô 
enableSslFlag
ôô 
;
ôô 
if
öö 
(
öö 
bool
öö 
.
öö 
TryParse
öö 
(
öö 
enableSslFlagRaw
öö .
,
öö. /
out
öö0 3
enableSslFlag
öö4 A
)
ööA B
)
ööB C
{
õõ 

smtpClient
úú 
.
úú 
SetEnableSsl
úú '
(
úú' (
enableSslFlag
úú( 5
)
úú5 6
;
úú6 7
}
ùù 
var
†† 

smtpServer
†† 
=
†† 
GetProperty
†† (
<
††( )
string
††) /
>
††/ 0
(
††0 1
config
††1 7
,
††7 8
$str
††9 E
)
††E F
;
††F G
if
°° 
(
°° 
string
°° 
.
°° 
IsNullOrEmpty
°° $
(
°°$ %

smtpServer
°°% /
)
°°/ 0
==
°°1 3
false
°°4 9
)
°°9 :
{
¢¢ 

smtpClient
££ 
.
££ 
SetSmtpServer
££ (
(
££( )

smtpServer
££) 3
)
££3 4
;
££4 5
}
§§ 
var
ßß 
smtpPortRaw
ßß 
=
ßß 
GetProperty
ßß )
<
ßß) *
string
ßß* 0
>
ßß0 1
(
ßß1 2
config
ßß2 8
,
ßß8 9
$str
ßß: D
)
ßßD E
;
ßßE F
int
®® 
smtpPort
®® 
;
®® 
if
©© 
(
©© 
int
©© 
.
©© 
TryParse
©© 
(
©© 
smtpPortRaw
©© (
,
©©( )
out
©©* -
smtpPort
©©. 6
)
©©6 7
)
©©7 8
{
™™ 

smtpClient
´´ 
.
´´ 
SetSmtpPort
´´ &
(
´´& '
smtpPort
´´' /
)
´´/ 0
;
´´0 1
}
¨¨ 
var
ØØ #
smtpDeliveryMethodRaw
ØØ %
=
ØØ& '
GetProperty
ØØ( 3
<
ØØ3 4
string
ØØ4 :
>
ØØ: ;
(
ØØ; <
config
ØØ< B
,
ØØB C
$str
ØØD X
)
ØØX Y
;
ØØY Z
if
∞∞ 
(
∞∞ 
string
∞∞ 
.
∞∞ 
IsNullOrEmpty
∞∞ $
(
∞∞$ %#
smtpDeliveryMethodRaw
∞∞% :
)
∞∞: ;
==
∞∞< >
false
∞∞? D
)
∞∞D E
{
±± 
var
≤≤  
smtpDeliveryMethod
≤≤ &
=
≤≤' (
(
≤≤) * 
SmtpDeliveryMethod
≤≤* <
)
≤≤< =
Enum
≤≤= A
.
≤≤A B
Parse
≤≤B G
(
≤≤G H$
SmtpDeliveryMethodType
≤≤H ^
,
≤≤^ _#
smtpDeliveryMethodRaw
≤≤` u
)
≤≤u v
;
≤≤v w

smtpClient
≥≥ 
.
≥≥ #
SetSmtpDeliveryMethod
≥≥ 0
(
≥≥0 1 
smtpDeliveryMethod
≥≥1 C
)
≥≥C D
;
≥≥D E
}
¥¥ 
var
∑∑ %
pickupDirectoryLocation
∑∑ '
=
∑∑( )
GetProperty
∑∑* 5
<
∑∑5 6
string
∑∑6 <
>
∑∑< =
(
∑∑= >
config
∑∑> D
,
∑∑D E
$str
∑∑F _
)
∑∑_ `
;
∑∑` a
if
∏∏ 
(
∏∏ 
string
∏∏ 
.
∏∏ 
IsNullOrEmpty
∏∏ $
(
∏∏$ %%
pickupDirectoryLocation
∏∏% <
)
∏∏< =
==
∏∏> @
false
∏∏A F
)
∏∏F G
{
ππ 

smtpClient
∫∫ 
.
∫∫ (
SetPickupDirectoryLocation
∫∫ 5
(
∫∫5 6%
pickupDirectoryLocation
∫∫6 M
)
∫∫M N
;
∫∫N O
}
ªª 
var
ææ 

timeoutRaw
ææ 
=
ææ 
GetProperty
ææ (
<
ææ( )
string
ææ) /
>
ææ/ 0
(
ææ0 1
config
ææ1 7
,
ææ7 8
$str
ææ9 B
)
ææB C
;
ææC D
int
øø 
timeout
øø 
;
øø 
if
¿¿ 
(
¿¿ 
int
¿¿ 
.
¿¿ 
TryParse
¿¿ 
(
¿¿ 

timeoutRaw
¿¿ '
,
¿¿' (
out
¿¿) ,
timeout
¿¿- 4
)
¿¿4 5
)
¿¿5 6
{
¡¡ 

smtpClient
¬¬ 
.
¬¬ 

SetTimeout
¬¬ %
(
¬¬% &
timeout
¬¬& -
)
¬¬- .
;
¬¬. /
}
√√ 
base
∆∆ 
.
∆∆ 
	Configure
∆∆ 
(
∆∆ 
config
∆∆ !
)
∆∆! "
;
∆∆" #
}
«« 	
public
…… 
void
…… 
	Configure
…… 
(
…… 
TargetConfig
…… *
config
……+ 1
,
……1 2
ILayoutFactory
……3 A
layoutFactory
……B O
)
……O P
{
   	
var
ÃÃ 

bodyFormat
ÃÃ 
=
ÃÃ 
GetProperty
ÃÃ (
<
ÃÃ( )
string
ÃÃ) /
>
ÃÃ/ 0
(
ÃÃ0 1
config
ÃÃ1 7
,
ÃÃ7 8
$str
ÃÃ9 ?
)
ÃÃ? @
;
ÃÃ@ A
if
ÕÕ 
(
ÕÕ 
string
ÕÕ 
.
ÕÕ 
IsNullOrEmpty
ÕÕ $
(
ÕÕ$ %

bodyFormat
ÕÕ% /
)
ÕÕ/ 0
)
ÕÕ0 1
{
ŒŒ 

bodyFormat
œœ 
=
œœ %
DefaultBodyLayoutFormat
œœ 4
;
œœ4 5
}
–– 
this
—— 
.
—— 

BodyLayout
—— 
=
—— 
layoutFactory
—— +
.
——+ ,
	GetLayout
——, 5
(
——5 6

bodyFormat
——6 @
)
——@ A
;
——A B
var
‘‘ 
subjectFormat
‘‘ 
=
‘‘ 
GetProperty
‘‘  +
<
‘‘+ ,
string
‘‘, 2
>
‘‘2 3
(
‘‘3 4
config
‘‘4 :
,
‘‘: ;
$str
‘‘< E
)
‘‘E F
;
‘‘F G
if
’’ 
(
’’ 
string
’’ 
.
’’ 
IsNullOrEmpty
’’ $
(
’’$ %
subjectFormat
’’% 2
)
’’2 3
==
’’4 6
false
’’7 <
)
’’< =
{
÷÷ 
this
◊◊ 
.
◊◊ 
SubjectLayout
◊◊ "
=
◊◊# $
layoutFactory
◊◊% 2
.
◊◊2 3
	GetLayout
◊◊3 <
(
◊◊< =
subjectFormat
◊◊= J
)
◊◊J K
;
◊◊K L
}
ÿÿ 
else
ŸŸ 
{
⁄⁄ 
throw
€€ 
new
€€ '
InvalidOperationException
€€ 3
(
€€3 4
$str€€4 ¢
)€€¢ £
;€€£ §
}
‹‹ 
}
›› 	
public
ﬂﬂ 
override
ﬂﬂ 
void
ﬂﬂ 
Dispose
ﬂﬂ $
(
ﬂﬂ$ %
)
ﬂﬂ% &
{
‡‡ 	
if
‚‚ 
(
‚‚ (
DisposeSmtpClientOnDispose
‚‚ *
)
‚‚* +
{
„„ 

smtpClient
‰‰ 
.
‰‰ 
Dispose
‰‰ "
(
‰‰" #
)
‰‰# $
;
‰‰$ %
}
ÂÂ 
base
ËË 
.
ËË 
Dispose
ËË 
(
ËË 
)
ËË 
;
ËË 
}
ÈÈ 	
}
ÍÍ 
}ÎÎ ˇ
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
}HH ó
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
}NN õ
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