using Core;
using Core.Application.Customers.Add;
using Core.Application.Customers.Details;
using Core.Application.Customers.List;
using Core.Application.Products.List;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace task.Controllers;

public class CustomersController(IMediator mediator, IValidator<AddCustomerCommand> validator) : Controller
{
    [HttpGet(Routes.Customer.List)]
    public async Task<IActionResult> List([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetCustomersListQuery(), cancellationToken);
        return View(result);
    }

    [HttpGet(Routes.Customer.Details)]
    public async Task<IActionResult> Details([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetCustomerDetailsQuery(id), cancellationToken);
        return View(result);
    }

    [HttpGet(Routes.Customer.Add)]
    public async Task<IActionResult> Add(CancellationToken cancellationToken = default)
    {
        ViewData[$"{nameof(AddCustomerCommand.Products)}"] =
            await LoadProductsSelectListItemAsync(cancellationToken);

        var customerProducts = new List<CustomerProductsModel>();
        HttpContext.Session.SetString(nameof(CustomerProductsModel), JsonSerializer.Serialize(customerProducts));

        return View(new AddCustomerCommand());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddCustomerCommand command,
        CancellationToken cancellationToken = default)
    {
        var customerProducts =
            JsonSerializer.Deserialize<List<CustomerProductsModel>>(
                HttpContext.Session.GetString(nameof(CustomerProductsModel))!);

        customerProducts!.ForEach(x => command.Products.Add(new CustomerProductsModel
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price
        }));

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(x =>
                ModelState.AddModelError(key: x.PropertyName, errorMessage: x.ErrorMessage));

            ViewData[$"{nameof(AddCustomerCommand.Products)}"] =
                await LoadProductsSelectListItemAsync(cancellationToken);

            return View(nameof(Add), command);
        }

        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProducts(int productId, decimal productPrice,
        CancellationToken cancellationToken = default)
    {
        if (productPrice <= 0)
            return BadRequest();

        var products = await mediator.Send(new GetProductsListQuery(), cancellationToken);
        var product = products.FirstOrDefault(x => x.Id == productId);

        if (product is null)
            return BadRequest();

        var customerProducts =
            JsonSerializer.Deserialize<List<CustomerProductsModel>>(
                HttpContext.Session.GetString(nameof(CustomerProductsModel))!);

        if (customerProducts == null)
            return BadRequest();

        var customerProduct = customerProducts.Find(x => x.Id == productId);
        if (customerProduct is not null)
        {
            customerProduct.Price = productPrice;
        }
        else
        {
            customerProducts.Add(new CustomerProductsModel
            {
                Id = productId,
                Price = productPrice,
                Name = product.Name
            });
        }

        HttpContext.Session.SetString(nameof(CustomerProductsModel), JsonSerializer.Serialize(customerProducts));

        return PartialView("_ProductListPartial", customerProducts);
    }

    private async Task<List<SelectListItem>> LoadProductsSelectListItemAsync(CancellationToken cancellationToken = default)
    {
        var products = await mediator.Send(new GetProductsListQuery(), cancellationToken);
        return products.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id.ToString()
        }).ToList();
    }
}