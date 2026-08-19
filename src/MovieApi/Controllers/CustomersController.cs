using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Constants;
using MovieApi.Contracts.Customers;
using MovieApi.Mapping;
using MovieApi.Repositories;

namespace MovieApi.Controllers;

[ApiController]
[Authorize(Roles = UserRoles.AdminOrUser)]
[Route("api/customers")]
public sealed class CustomersController(ICustomerRepository customers) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<CustomerResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IReadOnlyCollection<CustomerResponse>> GetAll()
    {
        var response = customers.GetAll().Select(customer => customer.ToResponse()).ToArray();
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<CustomerResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CustomerResponse> GetById(Guid id)
    {
        var customer = customers.GetById(id);
        return customer is null ? NotFound() : Ok(customer.ToResponse());
    }

    [HttpPost]
    [ProducesResponseType<CustomerResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CustomerResponse> Create(CustomerRequest request)
    {
        var customer = customers.Add(request.FullName, request.Email, request.PhoneNumber);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer.ToResponse());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, CustomerRequest request)
    {
        return customers.Update(id, request.FullName, request.Email, request.PhoneNumber)
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return customers.Delete(id) ? NoContent() : NotFound();
    }
}
