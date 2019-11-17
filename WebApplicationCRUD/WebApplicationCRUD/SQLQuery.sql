create database aspSekolah
use aspsekolah

create table SubjectDetails (
SubjectId int identity(1,1)  primary key,
SubjectName varchar(20),
Marks float, 
Grade varchar(1)
)

select * from subjectdetails