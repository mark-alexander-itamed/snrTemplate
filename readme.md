# Freightliner Developer Technical Challenge

This technical challenge is meant to test your comfort programming
in a similar environment to the day-to-day operations at Freightliner.
It isn't a quiz of obscure C# exception handling features, nor will we 
ask you to invert a binary tree. There is no set time limit for this task.

This repo contains a simple single-page-application written in react.js
and Asp.Net 5.0. It's already in an operational state, and after completing
the setup you should be able to use the program.

Please read these instructions carefully before proceeding.

The app is an extremely simple bug tracker. It has few features:

- You can report bugs.
- You can browse existing bugs.
- You can view a bug in detail.
- You can set a bug to one of two states, being 'Open' and 'Closed'.
- You will be able to add comments to a bug.

Your task is to complete the following steps:

- Get the app up and running locally. This requires node.js, version 14 and .Net 5.0, 
  installation instructions are below.
- Complete the 'Comment' functionality. This is currently part-complete, 
  with a working test suite but no implementation.
- There is an optional final task described at the bottom of this document. This step, should you choose to complete it will take a few hours. It is designed to allow you to demonstrate your comfort in maintaining older applications.

When you've finished, push your final version. If you have any comments you'd
like to leave, create a new file in the repo containing these comments,
and we'll make sure they're read.

## Assessment criteria

- **Maintaining consistent code style within a project.** This is a part complete project,
  and we'd like the code to use similar idioms to those present.
- **Descriptive and coherent variable naming.**
- **Consideration of edge cases and potential error cases.**
- **Size of git commits, and quality of commit messages.** We're looking for short, 
  simple messages that describe what you did and why.
  

## Setup instructions

You'll need the following installed:

- git (https://git-scm.com/)
- .Net 5.0 SDK (https://dotnet.microsoft.com/download/dotnet/5.0)
- Node.js, any recent version (nodejs.org/en)
- Any text editor that can work with .Net 5.0 and React:
    - Any edition of Visual Studio 2019 or newer (very good with C#, ok with React)
    - JetBrains Rider 2020.3 or later (£££, but very good with C# and React)
    - Visual Studio Code (free/open source, pretty good with C# and React if the right plugins are installed)
    
Check these are installed with the following commands:

```powershell
dotnet --list-sdks   # Should include '5.0' in the list
node --version       # Should return a version number.
```

## Getting the source code, and submitting the test

You can obtain the source code by cloning the github repository. You will have read/write
access to this repo, so you will be able to push when you're done.

The only branch on the repository should be 'test'. You shouldn't make your changes directly
in this branch, so *create a new branch before starting*.

When you're done, push your new branch up to github using `git push -u origin <your branch name>`.
Then, in github, create a pull request back into the 'test' branch with your changes.

If you choose to complete the optional, final task, please create a separate pull request for this. It will allow us to assess the two tasks separately.

## Running the project

These instructions assume you're operating from a CLI. You can configure your IDE to build
and run, but that's IDE specific, while terminal commands are generic.

First check the tests are running. In a CLI window in the root of the repository (where
BugReporter.sln is located):

```powershell
dotnet test
```

This should perform the 'dotnet restore' for you.

The tests should all run, but they shouldn't all pass!

To run the web api:

```powershell
cd server/src/BugReporter.Api
dotnet run --launch-profile "BugReporter.Api"
```

Open a browser at http://localhost:5000/swagger/index.html. You should be looking
at an overview of the API. You can use this interface to play with the API without
writing a frontend or using a tool like postman.

Now, set up and start up the UI. In a new CLI, navigate to /app. You should be in
the same folder as the `package.json` file.

```powershell
npm install
npm start
```

This might take a few minutes, particularly the install script. Once it's installed
and running, a browser window will open at http://localhost:3000. This is the React
app, running in a development server. If you make any changes to the React code,
the app will automatically be refreshed.

## Developing

The app uses a sqlite database to store data. To make things easier in the case of 
schema changes, the app will recreate the database every time you restart it. If you
don't want this to happen, comment out line 24 of Program.cs. 

To keep things simple, we're not using migrations on this app. The database is generated
by EntityFramework once, but it can't detect or deploy changes to the schema. If you make
any schema changes, such as changing a column or adding a table, you'll need to restart
the database. If you haven't commented out the line in Program.cs, this will happen automatically.
If you have, just delete Bug.db and a new database will be created when you restart.

The React app uses Material UI, a large component library based on Google's Material Design.
If you've used any other Material library, such as Angular Material or Materialize.css, you'll
probably be familiar with these components.

The documentation for Material UI is at material-ui.com. Most of what you need is in the
left-hand sidebar, under the 'Components' tab. The 'Api' tab contains details about each 
component, including documentation for their properties.

For form handling, this app uses 'Formik'. Formik has been the accepted standard for React
form handling for a while now, and is well documented at https://formik.org/docs/overview.
The forms are validated with [Yup](https://github.com/jquense/yup), which integrates extremely
well with Formik.

## Walkthrough

On the app, create a new bug. Use the button in the top right (Report A Bug) to do this.
When you've created a bug, you'll be forwarded to a 'details page', with the information
you used. You can update the 'State' using the form on the left.

Try to add a comment to the bug. You shouldn't be able to - you should see an error message
appear in the top-right of the screen.

### Problem 1: Enable comments

This feature hasn't been completely implemented. We've written tests for it though,
so if you run `dotnet test` on the solution you should see some failing test cases.

Your first task is to implement the service logic for adding a comment. All tests
should pass when you're done. You shouldn't need to change any existing tests, but
you may add more if you feel they're required.

Once it's done, test it out by creating a few comments.

### Problem 2: Comment identification

The comments seem a little... sparse. Right now they're fine while you're the only
person using the application, but with more users that might prove tricky.

Please add a 'Commenter' field to the comment. You'll need to make changes on both
sides the app and the API for this. Make sure you write unit tests covering the
changing behaviour, but you shouldn't need to change any of the existing ones.

We'd like to see:

- A new field on the form labelled 'Your name'. 
- Validation on both the client and the server.
- Unit tests clearly documenting the new functionality.

Here's some links that might be helpful:

- Formik (form handling): https://formik.org/docs/overview
- Yup (client form validation): https://github.com/jquense/yup
- Material UI text field API reference: https://material-ui.com/api/text-field/

### Problem 3: A small redesign

The component 'Comment' (src/App/ViewBug/Comment.tsx) doesn't look great. Right now
it doesn't even display the commenter's name we added in the previous step.

You should redesign this comment however you like. You should use the Material UI components
for this. A more detailed description of the requirements is a comment in the source code.

Some tips:
- If you want to change the date formatting, the `DateUtils` object is just a container for
  the `date-fns` library. You can use any function from there.
- You can use your own css. Notice at the bottom of some of the components there's a section
  that reads `const useStyles = makeStyles(theme => createStyles({...}))`, which is then called
  in the components with `const classes = useStyles()`. You can use this object to create css
  classes.

### Problem 4: Upgrade to the latest verison of .Net and either the Current LTS or Latest version of Node.JS

This task is optional and is designed to test your knowledge in application maintenance. It will probably take you a few hours to complete.
Hints: 
-  You will need to uninstall Node 14 and install your chosen verison of node. 
-  Some of the libraries have changed names. Be sure to follow the migration documentation, it will save you a lot of time and frustration.

This application uses out of date versions of both .Net and Node.Js/React.

We would like for it to run on the current version of .Net (6 at the time of writing) and one of:
  - The current LTS version of Node.js (16 at the time of writing)
  - The 'Latest' version of Node.Js (18 at the time of writing)

Upgrade the .Net project first, and ensure it runs, is still functional and that the Unit tests run
Install the node version of your choice, then ensure the react application still works.

In your pull request, please note which version of Node.js and .Net you have used, so that we are able to 
reproduce your results properly.

    


