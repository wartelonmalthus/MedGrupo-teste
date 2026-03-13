using MedGrupo_teste.Application.Common;
using MedGrupo_teste.Application.DTOs;
using MedGrupo_teste.Application.Services;
using MedGrupo_teste.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace MedGrupo_teste.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContactsController : ControllerBase
{
    private readonly ContactService _contactService;

    public ContactsController(ContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContactResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var contacts = await _contactService.ListActiveAsync(cancellationToken);
        return Ok(contacts);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var contact = await _contactService.GetActiveByIdAsync(id, cancellationToken);
            return Ok(contact);
        }
        catch (NotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ContactRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var createdContact = await _contactService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = createdContact.Id }, createdContact);
        }
        catch (DomainValidationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ContactRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var updatedContact = await _contactService.UpdateAsync(id, request, cancellationToken);
            return Ok(updatedContact);
        }
        catch (DomainValidationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (NotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _contactService.DeactivateAsync(id, cancellationToken);
            return NoContent();
        }
        catch (NotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _contactService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (NotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}
