using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Tp2.Controllers.QueryParameters;
using Tp2.Dtos;
using Tp2.Extensions;
using Tp2.repositories;

namespace Tp2.Controllers
{
    [Route("api/owners")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public class OwnersController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public LinkGenerator _linkGenerator { get; }

        public OwnersController(IRepositoryWrapper repositoryWrapper, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public IActionResult GetOwners([FromQuery] OwnerQueryStringParams ownerParameters)
        {
            if (!ownerParameters.ValidYearRange)
            {
                return BadRequest("Max year of birth cannot be less than min year of birth");
            }

            var owners = _repositoryWrapper.Owner.GetOwners(ownerParameters);
            var ownersDto = _mapper.Map<IEnumerable<Owner>,IEnumerable<OwnerDto>>(owners);
            var metadata = new
            {
                owners.TotalCount,
                owners.PageSize,
                owners.CurrentPage,
                owners.TotalPages,
                owners.HasNext,
                owners.HasPrevious
            };
            var shapedOwnersDto = ownersDto.SelectByFieldsQuery(ownerParameters.fields);
            foreach(ExpandoObject expandoOwner in shapedOwnersDto)
            {
                var dict = ((IDictionary<string, object>)expandoOwner);
                Object value;
                dict.TryGetValue("OwnerId", out value);
                dict.Add("links", CreateLinksForOwner((Guid)value));
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            LinkCollectionWrapper collectionWrapper = new LinkCollectionWrapper()
            {
                Value = shapedOwnersDto.ToList(),
                Links = CreateLinksForOwners(ownerParameters.fields)
            };
            return Ok(collectionWrapper);
        }


        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOwnerById(Guid id, [FromQuery] OwnerQueryStringParams ownerParameters)
        {
            try
            {
                var owner = _repositoryWrapper.Owner.GetOwnerById(id);

                if (owner == null)
                {
                    return NotFound();
                }
                else
                {

                    var ownerDto = _mapper.Map<OwnerDto>(owner);
                    return Ok(ownerDto.shapeObject(ownerParameters.fields));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public IActionResult Post([FromBody] OwnerForCreation ownerForCreation)
        {
            try
            {
                if (ownerForCreation == null)
                {
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var ownerEntity = _mapper.Map<Owner>(ownerForCreation);

                _repositoryWrapper.Owner.CreateOwner(ownerEntity);
                _repositoryWrapper.Save();

                var createdOwner = _mapper.Map<OwnerDto>(ownerEntity);

                return CreatedAtRoute("GetById", new { id = ownerEntity.OwnerId }, createdOwner);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Update(Guid id, [FromBody]OwnerForUpdate ownerForUpdate)
        {
            try
            {
                if (ownerForUpdate == null)
                {
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var owner = _repositoryWrapper.Owner.GetOwnerById(id);
                if (owner == null)
                {
                    return NotFound();
                }

                _mapper.Map(ownerForUpdate, owner);
                _repositoryWrapper.Owner.UpdateOwner(owner);
                _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult Delete(Guid guid)
        {

            try {
                var owner = _repositoryWrapper.Owner.GetOwnerById(guid);
                if (owner == null)
                {
                    return NotFound();
                }
                else
                {
                    if (owner.Accounts.Any())
                    {
                        return BadRequest("this user has accounts ,delete them  and try again");
                    }
                    else
                    {
                        _repositoryWrapper.Owner.DeleteOwner(owner);
                        _repositoryWrapper.Save();
                        return NoContent();
                    }
                }
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");

            }

        }

        private IEnumerable<Link> CreateLinksForOwner(Guid id, string fields = "")
        {
            
            var links = new List<Link>
                                    {
                                    new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetOwnerById), values: new { id, fields }),
                                    "self",
                                    "GET"),

                                    new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(Delete), values: new { id }),
                                    "delete_owner",
                                    "DELETE"),

                                    new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(Update), values: new { id }),
                                    "update_owner",
                                    "PUT")
                                    };

            return links;
        }
        private List<Link> CreateLinksForOwners(string fields = "")
        {

            return new List<Link>{
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetOwners), values: new {fields}),"self","GET")
            };
        
        }
        
    }
}