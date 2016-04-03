using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVCWeb.Controllers
{
    public class PersonApiController : ApiController
    {
        // GET: api/Persons
        [System.Web.Http.Route("api/Persons")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PersonApi/5
        public string Get(int id)
        {
            return "value" + id;
        }

        // GET: api/PersonByName/?name=Ramu // Url nav in browser got Xml by default, coz Accept would be Html/Xml etc
        [System.Web.Http.Route("api/PersonByName")]
        public Person Get(string name)
        {
            return new Person
            {
                Id = 1, Name = "Found Person with a name: " + name
            };
        }

        //[System.Web.Http.HttpGet] // Http ns or Mvc ?? 
        [System.Web.Http.Route("api/PersonByName2")]
        public HttpResponseMessage GetPersonApimmmm(string name)
        {
            //Request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //Request.Content.Headers.Add("Content-Type", "application/json");
            var response = 
            Request.CreateResponse(
                HttpStatusCode.OK, new Person
                {
                    Id = 1,
                    Name = "Found Person with a name: " + name,
                    Addresses = new List<Address>
                    {
                        new Address
                        {
                            AddressId = 1,
                            AddressLine1 = "1 High Street",
                            City = "Daventry",
                            State = States.Northamptonshire
                        },
                    }
                });
            response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json"); // force json response
            return response;
        }

        // POST: api/PersonApi
        // [Route("api/books/{id:int}")]
        // Body contains {"Id": "0", "Name": "Peraveen"}
        [System.Web.Http.Route("api/PostPerson")]
        public IHttpActionResult Post([FromBody]JToken value)  // bad  :: No Model Validation performed
        {
            // TO DO: Not firing validation... Boo hooo !! Damn MS..
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // if we get null here it is clarified here:
            //http://bizcoder.com/posting-raw-json-to-web-api
            // Solution 1:
            /*
             * public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
             * {    
             *      var jsonString = await request.Content.ReadAsStringAsync(); and JsonConvert.DeserializeObject<type>
             *              like JsonConvert.DeserializeObject<string>(jsonBody) // string param
             *      
             * Solution 2:
             * The JSON.Net deserializer will happily convert any arbitrary JSON document into a JToken instance.
             *  public HttpResponseMessage Post([FromBody]JToken jsonbody)
             *      can call ToString on jsonbody
             */
            
            //return new Task<TextResult>(() =>
            //{
                var person = value.ToObject<Person>();
            person.Addresses = new List<Address>
            {
                new Address
                {
                    AddressId = 1,
                    AddressLine1 = "1 High Street",
                    City = "Daventry",
                    State = States.Northamptonshire
                },
                new Address {AddressId = 2, AddressLine1 = "20 Main Street", City = "Irvine", State = States.Ayrshire}
            };
                var personJson = JsonConvert.SerializeObject(person);

                // Request is of type HttpRequestMessage
                var txtResult = new TextResult(personJson, Request, HttpStatusCode.Accepted);
                return txtResult;
            //});

            //{
            //    StatusCode = HttpStatusCode.Created,
            //    Content = new StringContent(personJson)
            //};
        }

        [System.Web.Http.HttpPost]
        // Body contains {"Id": "0", "Name": "Peraveen"}
        [System.Web.Http.Route("api/CreatePerson")]
        public HttpResponseMessage CreatePerson([FromBody]Person person)
        {
            var personJson = JsonConvert.SerializeObject(person);
            return Request.CreateResponse(
                HttpStatusCode.Created, personJson);
       }

        // PUT: api/PersonApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PersonApi/5
        public void Delete(int id)
        {
        }
    }

    // http://www.asp.net/web-api/overview/getting-started-with-aspnet-web-api/action-results
    public class TextResult : IHttpActionResult
    {
        string _value;
        HttpRequestMessage _request;
        HttpStatusCode _code;

        public TextResult(string value, HttpRequestMessage request, HttpStatusCode code)
        {
            _value = value;
            _request = request;
            _code = code;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(_code, _value); // use content negotiation
            return Task.FromResult(response);
        }
    }
}
