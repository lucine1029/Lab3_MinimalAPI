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
        public static IResult AddNewPerson(ApplicationContext context, PersonDto personDto)
        {
            context.Person.Add(new Models.Person()
            {
                FirstName = personDto.FirstName,
                LastName = personDto.LastName,
                PhoneNum = personDto.PhoneNum
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


        //--------- Interest
        public static IResult AddNewInterest(ApplicationContext context, InterestDto interestDto)
        {
            //add new interest to the database
            context.Interests.Add(new Models.Interest()
            {
                InterestName = interestDto.InterestName,
                InterestDescription = interestDto.InterestDescription
            });
            context.SaveChanges();
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ConnectInterestToPerson(ApplicationContext context, int personId, InterestDto interestDto)
        {
            //Connect a person to an new interest
            //1. find the person with the given Id
            var person = context.Person
                .Where(p => p.Id == personId)
                .Include(p => p.Interests)
                .SingleOrDefault();
            if (person == null) 
            { 
                return Results.NotFound();
            }
            //2. add an new interest to the database
            var newInterest = new Models.Interest()
            {
                InterestName = interestDto.InterestName,
                InterestDescription = interestDto.InterestDescription
            };
            context.SaveChanges();
            return Results.StatusCode((int)HttpStatusCode.Created);

            //3. add the new interest to the person
            person.Interests.Add(newInterest);
            context.SaveChanges();
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ListPersonsInterests(ApplicationContext context, int personId)
        {
            //list all interests of a person with a given id in a Viewmodel
            var person = context.Person
                .Where(p => p.Id == personId)
                .Include(p => p.Interests)
                .SingleOrDefault();

            if (person == null)
            {
                return Results.NotFound($"The person with {personId} has not found.");
            }

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

            if (person == null)
            {
                return Results.NotFound($"The person with {personId} has not found.");
            }

            List<ListPersonsLinksViewModel> links = person.InterestLinks.Select(l => new ListPersonsLinksViewModel
            {
                URL = l.URL
            }).ToList();
            return Results.Json(links);
        }
        public static IResult InsertLinkToPersonToInterest(ApplicationContext context, InterestLinkDto interestLinkDto, int personId, int interestId)
        {
            //Insert new links for a specific person and a specific interest
            //find the person with the given Id
            var person = context.Person
                .Where(p => p.Id == personId)
                .SingleOrDefault();
            if (person == null)
            {
                return Results.NotFound($"The person with {personId} has not found.");
            }
            //find the interest with the given Id
            var interest = context.Interests
                .Where(i => i.Id == interestId)
                .SingleOrDefault();
            if (interest == null)
            {
                return Results.NotFound($"The interest with {interestId} has not found.");
            }
            //create an new link
            var link = new InterestLink
            {
                URL = interestLinkDto.URL
            };
            //connect the new link to the person and the interest
            person.InterestLinks.Add(link);
            interest.InterestLinks.Add(link);
            context.SaveChanges();
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
