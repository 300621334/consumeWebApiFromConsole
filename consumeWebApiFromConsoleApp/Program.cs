using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

/*http://www.tutorialsteacher.com/webapi/consuming-web-api-in-dotnet-using-httpclient
 Open NuGet Package Manager console from TOOLS -> NuGet Package Manager -> Package Manager Console and execute following command.

Install-Package Microsoft.AspNet.WebApi.Client
 */

namespace consumeWebApiUsingHttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1 for GET 2 for POST");
            int x = Convert.ToInt32(Console.ReadLine());
            if(x==1)
            {
                #region GET
               

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:56190/api/");
                    //HTTP GET
                    var responseTask = client.GetAsync("student");  //The GetAsync() method sends an http GET request to the specified url. The GetAsync() method is asynchronous and returns a Task. 
                    responseTask.Wait();                            //Task.wait() suspends the execution until GetAsync() method completes the execution and returns a result. http://www.tutorialsteacher.com/webapi/consuming-web-api-in-dotnet-using-httpclient

                    var result = responseTask.Result;               //Once the execution completes, we get the result from Task using Task.result which is HttpResponseMessage. Now, you can check the status of an http response using IsSuccessStatusCode. Read the content of the result using ReadAsAsync() method.
                    if (result.IsSuccessStatusCode)
                    {
                        //When we send GET request to http://localhost:56190/api/student the GET() action-method returns a IList<StudentViewModel>
                        //Then WebApi engine converts(serialize) that list to a JSON or XML and sends over to this app
                        //then this app deSerialize that JSON/xml into objs of type "Student" w is a class in this app

                        var readTask = result.Content.ReadAsAsync<Student[]>();//could use IList<Student> instead of Student[]
                        readTask.Wait();

                        var students = readTask.Result;

                        foreach (var student in students)
                        {
                            Console.WriteLine(student.Name);
                        }
                    }

                }
                #endregion
            }
            if(x==2)
            {
                #region POST
                
                

                var student = new Student() { Name = "Steve"};
                using (var client = new HttpClient())
                {
                client.BaseAddress = new Uri("http://localhost:56190/api/");
                var postTask = client.PostAsJsonAsync<Student>("student", student);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsAsync<StudentReturned>();
                    readTask.Wait();

                    StudentReturned insertedStudent =   readTask.Result;

                    Console.WriteLine("Student {0} inserted with id: {1}", insertedStudent.StudentName, insertedStudent.StudentID);
                }
                
                    Console.WriteLine(result.StatusCode);
                }
                #endregion
            }
            else
            {
                Console.WriteLine("wrong selection");
            }
           
            Console.ReadLine();
        }//Main method ends   
        }//class Program ends
    }//Namespace ends
    //Model Class
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
       
    }
    public class StudentReturned
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
       
    }
    public class AddressViewModel
    {
       
    }
    public class StandardViewModel
    {
        
    }
