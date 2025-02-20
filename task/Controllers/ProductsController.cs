using Core;
using Core.Application.Products.Add;
using Core.Application.Products.List;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace task.Controllers;

public class ProductsController(IMediator mediator, IValidator<AddProductCommand> validator) : Controller
{
    [HttpGet(Routes.Product.List)]
    public async Task<IActionResult> List(CancellationToken cancellationToken = default)
    {
        var products = await mediator.Send(new GetProductsListQuery(), cancellationToken);
        return View(products);
    }

    [HttpGet(Routes.Product.Add)]
    public IActionResult Add()
    {
        var product = new AddProductCommand();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddProductCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(x =>
                ModelState.AddModelError(key: x.PropertyName, errorMessage: x.ErrorMessage));

            return View(nameof(Add), command);
        }

        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(List));
    }
}