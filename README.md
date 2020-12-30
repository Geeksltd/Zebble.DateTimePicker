[logo]: https://raw.githubusercontent.com/Geeksltd/Zebble.DateTimePicker/master/icon.png "Zebble.DateTimePicker"


## Zebble.DateTimePicker

![logo]

A Zebble plugin that allows the user to select a date or a time.


[![NuGet](https://img.shields.io/nuget/v/Zebble.DateTimePicker.svg?label=NuGet)](https://www.nuget.org/packages/Zebble.DateTimePicker/)

> It is initially displayed similar to a drop-down list control, showing the currently selected date or time (or the placeholder text). When the user taps it, it will then open a dialog to show day, month and year selection controls or Hour, Minute and optionally AM/PM selection controls.

<br>


### Setup
* Available on NuGet: [https://www.nuget.org/packages/Zebble.DateTimePicker/](https://www.nuget.org/packages/Zebble.DateTimePicker/)
* Install in your platform client projects.
* Available for iOS, Android and UWP.
<br>


### Api Usage

To use this plugin you can use below code:
```xml
<DatePicker Id="MyDatePicker" />

<TimePicker Id="MyTimePicker" />
```

<br>

#### DatePicker

You can define the text representation of the day value of DatePicker by setting DayFormat property. it is a standard day format string uses a single format specifier. For example, if you want to show Short date pattern in the datepicker(6/15/2009), you should set DayFormat property to "d", or if you want to show Long date pattern in the datepicker (Monday, June 15, 2009), you should set DayFormat property to "D".

##### DayFormat:

In case you want to change how a date picker show the date format you can use the following.
```xml
<DatePicker Id="MyDatePicker"   DayFormat="D" />

<DatePicker Id="MyDatePicker"   DayFormat="d" />
```
To see all supported standard .NET date formats see https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings.

##### MonthFormat:

In case you want to change how a date picker show the date format you can use the following.
```xml
<DatePicker Id="MyDatePicker"  MonthFormat = "MM" />

<DatePicker Id="MyDatePicker"  MonthFormat = "MMM" />
```
To see all supported standard .NET date formats see https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings.

##### TextFormat:

In case you want to change how a date picker show the date format you can use the following.
```xml
<DatePicker Id="MyDatePicker"   TextFormat="dd/MM/yyyy" />

<DatePicker Id="MyDatePicker"   TextFormat="dd/M/yyyy" />
```

### Properties
| Property     | Type         | Android | iOS | Windows |
| :----------- | :----------- | :------ | :-- | :------ |
| SelectedValue           | DateTime?           | x       | x   | x       |

### Events
| Event             | Type                                          | Android | iOS | Windows |
| :-----------      | :-----------                                  | :------ | :-- | :------ |
| SelectedValueChanged              | AsyncEvent    | x       | x   | x       |


#### TimePicker

It is initially displayed similar to a drop-down list control, showing the currently selected time (or the placeholder text). When the user taps it, it will then open a dialog to show Hour, Minute and optionally AM/PM selection controls.

You can optionally specify the TimeFormat and MinuteInterval properties.
```xml
<TimePicker Id="MyTimePicker" MinuteInterval="10" TimeFormat="@TimeFormat.Twentyfour" />
```
`Note:
You can set TimeFormat to AMPM or TwentyFour 
By default the value of MinuteInterval is 5`

##### SelectedValue:
By setting SelectedValue, you can determine a default value of TimePicker.
```xml
<TimePicker   SelectedValue="04:34PM"   />
```
##### SelectedText:
By setting SelectedText, you can determine a default text of TimePicker. As you know, each item of TimePicker list has a Value and a Text.
```xml
<TimePicker   SelectedText="05:45AM"  />
```

##### TextFormat:
```xml
<TimePicker  TextFormat="T"/>

<TimePicker  TextFormat="t"/>
```
To see all supported standard .NET time formats see https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings.

##### TimeFormat:
You can set TimeFormat to AMPM or TwentyFour.
```xml
<TimePicker Id="MyTimePicker"  TimeFormat="@TimeFormat.Twentyfour" />

<TimePicker Id="MyTimePicker"  TimeFormat="@TimeFormat.AMPM" />
```

### Properties
| Property     | Type         | Android | iOS | Windows |
| :----------- | :----------- | :------ | :-- | :------ |
| SelectedValue           | TimeSpan?           | x       | x   | x       |

### Events
| Event             | Type                                          | Android | iOS | Windows |
| :-----------      | :-----------                                  | :------ | :-- | :------ |
| SelectedValueChanged              | AsyncEvent    | x       | x   | x       |
