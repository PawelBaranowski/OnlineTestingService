OnlineTestingService
====================

*Project Scope*

The project is meant to provide a web interface for defining and performing online test for job candidates. Questions are open (the application does not support multiple choices) and answers are verified by a company’s employee. The candidate has limited time for each question (default is 5 minutes and this value can be changed while defining a question).

In more detail the project is going to include:
-> A website for candidates applying for a job. It allows them to fill the test (answer the questions). The test has time limitations. User has possibility to skip back and forward between questions.
-> Website application for company’s employees. It allows adding new candidates, creating questions, test templates, reviewing answers and grading them. Also some basic user management system is available. 

*Objective*

Performing online tests. Questions are open (the application does not support multiple choices) and answers are verified by a company’s employee. The candidate has limited time for each 2question (default is 5 minutes and this value can be changed while defining a question).


*Technology*:

ASP.NET MVC 3
SQL Server 2008
NHibernate

*Requirements*

- Adding candidates to the database
- Data stored: first name, last name, e-mail address, phone number, CV.
- Defining groups (categories) of questions
- Adding questions and assigning them to groups
- One question can belong to several groups. Question can be mandatory and has defined time for answer.
- Defining test templates
- Test template comprises of several groups of questions. For each group a number of questions to randomly choose is defined. The maximum number is number of questions in a group in the database. There is no minimum. Time limit for filling the test is defined by sum of all it’s questions (each question has it’s own time).
- Test generating
- Test is generated based on test templates which has specified question groups and number of questions from them. Test also has status (like filled, not filled, reviewed).
- Assigning a particular test template to a candidate
- A new test is created and given a unique ID. The candidate is notified by e-mail in which they receive an URL and password to access the test.
- Passwords should be safe from reading them in a bad way, i.e. characters ‘O’ and ‘0’, ‘1’ and ‘I’ look similar, so they shouldn’t be present in any password.
- Reviewing completed tests and adding comments to each answer by employees
- Each answer gets a grade. The overall test grade is the average of all grades.
- Administering system and data using web interface
- Any error prone data (such as e-mails and phone numbers) must be validated. Most of stored data must be filterable and sortable or searchable.
- Editing existing data
- Candidates’ info, test templates and notification templates can be edited or removed at will.
- Questions can be edited or removed but if a question was ever used in any test it must remain unchanged to make sure that “old” answers match the “old” questions.
- Previously created and completed test mustn’t be edited or removed.
- Sending e-mail notifications about completed tests
- Test template holds a list of e-mail addresses to send notifications to.
- Instead of sending an e-mail every time an event occurs, summary (hourly / daily / weekly / monthly) notifications should be available. A background service will be responsible for sending those. Frequency can be set by a system admin.
- Attaching a candidate’s CV to their info
- Any file type is available. Some reasonable limit of the file’s size should be introduced.
- Attaching a job offer to a test
- Same as above. It is a per-template attachment.
- Several types of roles (responsibilities) are available (see Actors section)
- A separate minimalistic web site used only for filling tests by candidates is available



*Actors and their goals*

- Candidate: Fills the test. They get a link to a test and a password and can fill the test at any time. It is allowed to close the browser window and open the test again. If there is still some time left they can continue filling the test.
- Test definer: Defines question groups, questions and test templates. Assigns questions to specific groups (one question can belong to several groups).
- Candidate manager: Assigns a test template to a particular candidate and adds new candidates to database.
- Test reviewer: Reviews, comments and grades answers.
- Admin: Creates new users and assigns roles to them, changes roles of existing users. Admin role includes all other roles.


*Limitations*

-> Both web sites must work properly in major modern web browsers.
->The answers must be saved frequently to avoid loss of candidate’s work.
->The testing system must instantly respond to actions.


