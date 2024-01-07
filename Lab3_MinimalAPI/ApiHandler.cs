using Lab3_MinimalAPI.Data;
using Lab3_MinimalAPI.Models;
using Lab3_MinimalAPI.Models.Dtos;
using Lab3_MinimalAPI.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Lab3_MinimalAPI
{
    public static class ApiHandler
    {
        //handling all API interactions

        //--------- Person
        public static IResult AddNewPerson(ApplicationContext context, PersonDto person)
        {
            context.Person.Add(new Models.Person()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                PhoneNum = person.PhoneNum
            });
            context.SaveChanges();
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ListAllPeople(ApplicationContext context)
        {
            //Lists all people from the database in a ViewModel
            List<ListAllPeopleViewModel> persons = context.Person.Select(p => new ListAllPeopleViewModel
            {
                FirstName = p.FirstName,
                LastName = p.LastName
            }).ToList();
            return Results.Json(persons);
        }
        public static IResult DeletePerson(ApplicationContext context, int personId)
        {
            var person = context.Person
                .Where(p => p.Id == personId)
                .FirstOrDefault();
            if (person != null)
            {
                context.Person.Remove(person);
                context.SaveChanges();
            }
            return Results.StatusCode((int)HttpStatusCode.Accepted);
        }

        //--------- Interest
        public static IResult AddNewInterest(ApplicationContext context, InterestDto interest)
        {
            //add new interest to the database
            context.Interests.Add(new Models.Interest()
            {
                InterestName = interest.InterestName,
                InterestDescription = interest.InterestDescription
            });
            context.SaveChanges();
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ConnectInterestToPerson(ApplicationContext context, int personId, int interestId)
        {

            //find the person with the given Id
            var person = context.Person
                .Where(p => p.Id == personId)
                .Include(p => p.Interests)
                .SingleOrDefault();
            if (person == null) 
            { 
                return Results.NotFound();
            }
             //find the interest with the given Id
            var interest = context.Interests
                .Where(i => i.Id == interestId)
                .SingleOrDefault();     
            if (interest == null)
            {
                return Results.NotFound();
            }
            //add the interest to the person
            person.Interests.Add(interest);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ListPersonsInterests(ApplicationContext context, int personId)
        {
            //list all interests of a person with a given id in a Viewmodel
            var person = context.Person
                .Where(p => p.Id == personId)
                .Include(p => p.Interests)
                .SingleOrDefault();

            List<ListPersonsInterestsViewModel> interests = person.Interests.Select(i => new ListPersonsInterestsViewModel
            {
                InterestName = i.InterestName,
                InterestDescription = i.InterestDescription
            }).ToList();
            return Results.Json(interests);

        }

        //--------- InterestLink
        public static IResult ListPersonsLinks(ApplicationContext context, int personId)
        {
            //list all interestLinks of a person with a given id in a Viewmodel
            var person = context.Person
                .Where(p => p.Id == personId)
                .Include(p => p.InterestLinks)
                .SingleOrDefault();

            List<ListPersonsLinksViewModel> links = person.InterestLinks.Select(l => new ListPersonsLinksViewModel
            {
                URL = l.URL
            }).ToList();
            return Results.Json(links);
        }
        public static IResult InsertLinkToPersonToInterest(ApplicationContext context, InterestLinkDto interestLink, int personId, int interestId)
        {
            //find the person with the given Id
            var person = context.Person
                .Where(p => p.Id == personId)
                .Include(p => p.Interests)
                .SingleOrDefault();
            if (person == null)
            {
                return Results.NotFound();
            }
            //find the interest with the given Id
            var interest = context.Interests
                .Where(i => i.Id == interestId)
                .SingleOrDefault();
            if (interest == null)
            {
                return Results.NotFound();
            }
            //create an new link
            var link = new InterestLink
            {
                URL = interestLink.URL
            };
            //connect the new link to the person and the interest
            person.InterestLinks.Add(link);
            interest.InterestLinks.Add(link);
            context.SaveChanges();
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
