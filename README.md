Create Repositories for performing CRUD operations on both department and employee controller
implement exception handling 
implement JWT token
create a login functionality for an employee
Steps to create login functionality
create a new table Users with following feilds Id, EmployeeId, Email, Password
in the employee table add one more feild email
when the user hit the login api it will check if the provided email and password by the employee exists in the Users table if yes then return a jwt token if not return unauthorized message
make every endpoint in the employee controller authorize so that employee can only access it using jwt token
jwt token should only be valid for 24 hours after that it should be expired
