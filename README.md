# OneiroDump

A C# cli tool that makes the process of documenting data related to sleep and dreams easier. 

# Features
- Asks questions from a configurable questionnaire (yaml)
- Saves answers to the path provided
- Six Question Types:
    - YesNo: Answer must be Yes or No. Can ask sub questions depending on the answer
    - Int: Answer must be an whole number. Can be defined to have a set range of valid numbers
    - Float: Answer must be a number. Like ints, can be defined to have a set range of valid numberse
    - String: Answer is a string
    - Time: Answer is a string that represents the time using the 24 hour clock (ex: "13:00")
    - Enum: Answer must be one of the defined possible answers

# TODO:

This project is being actively developed, meaning there are a few plans for the future.

- Saving to a sql database
- Fitbit integration (optional, of course)
- Improved formatting for the outputted files

# Dependencies

- Uses YamlDotNet for yaml parsing. Installed with dotnet add package YamlDotNet.

# MIT License

Any code in this repository is free to use, modify, and distribute in any way.
