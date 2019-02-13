# Contents
* [Project Configuration](#project-configuration)
* [Project Convention](#project-convention)
* [Test Convention](#test-convention)
* [Github Convention](#github-convention)
* [File Hierarchy](#file-hierarchy)
* [Team Work](#team-work)
* [Coding Convention](#coding-convention)
  * [General](#general)
  * [Naming](#naming)
  * [Formating](#formating)

## Convention

#### Project Convention

* everybody create a new brunch for his work
* one respons for one brunch
* if somebody want to do changes in someone's bruch he should create separate brunch
* all issues should have labels, assignees and should be bond to project
* all pull requests should have labels, assignees and should be bond to project

#### Test Convention

* one-unit test = one testing block
* if block testing depend on other objects, it should be mocked
* unit test can contain a single scenario, then the name of unit test should match with next pattern — TakeArgument_ReturnArgument_TestingBehaviour
* each scenario should match next template

```C#
[TestMethod]
public void TestingBlockName()
{
  // BEHAVIOR 
  // the beginning balance should be reduced by debit amount
  // TAKE
  // passed regular parameter
  // debit amount is less than beginnig balance
  // RETURN
  // a positive value after money has been dabit
  
  // Arrange
  double beginningBalance = 11.99;
  double debitAmount = 4.55;
  double expected = 7.44;
  BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

  // Act
  account.Debit(debitAmount);
  double actual = account.Balance;
  
  // Assert
  Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");
}
```

#### Github Convention

Branches name:

* Repository's branches
  - master — branch for clients
  - development — branch for developers

* Your branches
  - feature/FeatureName — required enhancement
  - fix/FixName — bug fix
  - topic/Name — optional enhancement

#### File Hierarchy

Unit-test and main project in one solution but in different projects.

Each project has the **"Resources"** , **"Models"** folder with inners folders
- Classes
- Interfaces
- EventArgClasses
- ExtensionClasses
- etc

And **"Components"** folder:
- Forms
- UserControls
- etc

Additional folders depend on projects.

### Coding Convention

#### General

- Configurable Data
  - do keep configurable data (using, const, static etc) at high levels
- Cycles
  - do use ++i not i++, iterator objects have different speed cost
- String
  - use StringBuilder for string manipulation
  - use all kind of string formating (String.Format(), @, $, {}, 0#.##, etc)
- Variable 
  - do not use *magic numbers*(*magic strings*), better use constants
- Fields
  - do not use public fields
  - distinguish constant and readonly
  - do not initialize fields in declaration, do use constructors
- Properties
  - do use read-only properties
  - do not use write-one properties, better use method
  - do use annonymous properties only in standalone, not important classes
  - if properties return field, it should has the same name
- Methods
  - one method = one action
  - method's body shoud be in 25-50 lines range
  - private methods for inner complex instructions
  - event methods should start with On- preffix
  - if variable names, that you pass, are not clear enough, use named parameters
- Events
  - **Two arguments**:
    - sender, object — object that has generated event
    - e — instance of EventArgs, contain event data
- Exception Handling
  - do not ignore cathed exception
    - some special exception can be ignored : ThreadAbortException
  - do hide errors from user, if can not handle exception show user a message
  - log exception in files with all details (type, time, method's name, class' name etc)
  - do not catch all exception, only specific ones
- Namespaces
  - Prefer full names to using command if you use namespace's objects infrequently
- Classes
  - do use partial classes for long classes files (over 10.000 lines)
    - fields, constructors in one part
    - methods, properties, indexers, events in another part
  - do not initialize class' properties after initialized instance, do use initialization by name
- Constructors
  - do initialize all fields explicitly
- Collections
  - do use only generic collection over object-based ones
- Files
  - each class in seperate file
  - file's name same as class' name
- Comments
  - do separate different code section with uppercase comments in important classes: FIELDS, METHODS, CONSTRUCTORS(constructor, destructor, methods that create object) ...
  - every *public* and *protected* blocks of code should have XML-documentation
  - every *private* block of code should has comments
  - avoid block of comments 
  - full sentence for comments
  - comments should be up in date
  - comments should be clear and understandable
  - single-line comments preffered over end-line ones
  - do use comments in complex block of codes

#### Naming

- "What?" not "How?"
- Make a sense
- Avoid negative conditional
```diff
- HasNotValue
+ HasValue
```
- Avoid redundency
```diff
- List.ListItem
+ List.Item
```
- Variable 
  - ‘i’, ‘j’, ’k’, ’l’, ’m’, ’n’  - cycles 
  - ‘x’, ‘y’, ‘z‘ -  coordinate
  - ‘r’, ‘g’, ‘b’  - colors
  - ‘e’  - events
  - ‘ex’  - exceptions
- GUI
  - use short suffix
    - Button —> btn
    - TextBox,RichTextBox —> tb
    - ComboBox, CheckBox —> cb
    - DataGridView —> dgv
  - if you don't use control, do not give him name, for inctance Splitter, Label
  - do use simple names for dynamic controls 
  

|  Object Name     |     Type     |  Notation  | Length | Plural | Prefix  | Suffix | Abbreviation | Char Mask   | Underscores |
|:-----------------|:------------:|:----------:|:------:|:------:|:-------:|:-------|:------------:|:------------|:-----------:|
| Assemblies       |Nouns(Company.Component)   | PascalCase | 50 | Y/N | No| No     | No           | [A-z]       | No          |
| Namespace        |Noun          | PascalCase |    50  | Y/N    | No      | No     | No           | [A-z]       | No          |
| Interface        |Noun or Nouns | PascalCase |    128 | No     | Yes     | No     | No           | {**I**}[A-z]| No          |
| Struct           |Noun or Nouns | PascalCase |    128 | No     | No      | Yes    | No           | [A-z][0-9]  | No          |
| Class            |Noun or Nouns | PascalCase |    128 | No     | No      | Yes    | No           | [A-z][0-9]  | No          |
| Constructor      |Same as class | PascalCase |    128 | No     | No      | Yes    | No           | [A-z][0-9]  | No          |
| Event Class      |Nouns         | PascalCase |    128 | No     | No      | Yes    | No           | [A-z][0-9]{EventArgs}| No |
| Attribute class  |Nouns         | PascalCase |    128 | No     | No      | Yes    | No           | [A-z][0-9]{Attribute}| No |
| Method           |Verbs         | PascalCase |    128 | Yes    | No      | No     | No           | [A-z][0-9]  | No          |
| Method arguments |Depend on type| camelCase  |    128 | Yes    | No      | No     | Yes          | [A-z][0-9]  | No          |
| Generic argument |Noun          | PascalCase |    50  | Yes    | No      | No     | Yes  |{**T**}[A-z][0-9]{Key,EventArgs}|No|
| Local variables  |Noun or Nouns | camelCase  |    50  | Yes    | No      | No     | Yes          | [A-z][0-9]  | No          |
| Constants name   |Noun or Nouns | UPPERCASE  |    50  | No     | No      | No     | No           | [A-z][0-9]  | Yes         |
| Field            |Noun or Nouns | camelCase  |    50  | Yes    | No      | No     | Yes          | [A-z][0-9]  | Yes         |
| Boolean Fields   |Noun or Nouns | camelCase  |    50  | Yes    | Yes     | No     | Yes    | {is,can,has,does+}[A-z][0-9]|Yes|
| Properties       |Same as field | PascalCase |    50  | Yes    | No      | No     | Yes          | [A-z][0-9]  | No          |
| Delegate         |Nouns         | PascalCase |    128 | No     | No      | Yes    | Yes          | [A-z]{EventHandler}| No   |
| Events           |Nouns         | PascalCase | 128    | No     | No      | Yes    | Yes          | [A-z]{Changed}| No        |
| Enum type        |Noun(regular) or Nouns(bit fields)|PascalCase|128|Yes|No|No     | No           | [A-z]         | No        |
| GUI              |Noun or Nouns |HungarianNotation|50 | Yes    | Yes     | Yes    | Yes          | [A-z]         | No        |
| GUI events       |ObjName + _ + EventName |PascalCase |128| No | No      | Yes    | Yes          | [A-z]{Changed}| Yes       |

#### Formating

* Padding
  - every block of code should has padding depending on outer block
  - padding should be done with tabulation
  - do use empty lines to divide logic
  - constanst and enums should be align on their types, names, operators etc
  ```C#
  public const int DBVERSION        = 4;
  public const int TINYINT_OWERFLOW = 8115;
  public const int TRIGGER_EXCEPT   = 50000;

  public enum StatusMode
  {
     		Planned  = 1,
     		Active   = 2,
     		InActive = 4,
     		All      = 7
  };

  ```
  - whitespaces after and before operators
  ```diff
  -isDisposing=false;
  +isDisposing = false;
  ```
  - long boolean statements should be divided by &&, || operators or incapsulated in variable or method
* curve brackets
  - every block of code (cycles, condition statements etc) should has curve brackets (exception underneath)
  - vertical brackets allignment
  ```diff
  - if (...){
  -}
  
  + if (...)
  +{
  +}
  ```
* code length
  - code line length should be less than 80 symbols
  - use shortage notation for short block of code
  - avoid curve brackets in short block of code
* one line = one command 
