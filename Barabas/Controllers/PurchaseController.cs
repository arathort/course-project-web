using Barabas.Models;
using Barabas.Services.EventService;
using Barabas.Services.OrderService;
using Barabas.Services.TicketService;
using Barabas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Barabas.Controllers
{
    [Authorize(Roles = "User, Manager, Admin")]
    public class OrdersController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ITicketService _ticketService;
        private readonly IOrderService _orderService;

        public OrdersController(IEventService eventService, ITicketService ticketService, IOrderService orderService)
        {
            _eventService = eventService;
            _ticketService = ticketService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> SelectTickets(int eventId)
        {
            var ev = await _eventService.GetEventById(eventId);

            var availableTickets = await _ticketService.GetAvailableTickets(eventId);

            var model = new TicketSelectionViewModel
            {
                EventName = ev.Name,
                AvailableSeats = availableTickets.Select(t => t.SeatNumber).ToList(),
                AvailableTicketIds = availableTickets.Select(t => t.Id).ToList() 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int ticketId, TicketSelectionViewModel model)
        {
            var ticket =await _ticketService.GetTicketById(ticketId);

            model.Cart.Add(ticket);

            return View("SelectTickets", model);
        }




        [HttpPost]
        public async Task<IActionResult> ProcessTicketSelection(string userId, int ticketId)
        {
            await _orderService.CreateOrder(userId, ticketId);

            return RedirectToAction("OrderSummary");
        }

        public IActionResult OrderSummary()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PurchasedTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name); 

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userOrders = await _orderService.GetOrdersByUserId(userId);

            var purchasedTickets = new List<PurchasedTicketViewModel>();

            foreach (var order in userOrders)
            {
                var ticket = await _ticketService.GetTicketById(order.TicketId);
                var eventDetails = await _eventService.GetEventById(ticket.EventId);

                purchasedTickets.Add(new PurchasedTicketViewModel
                {
                    TicketId = ticket.Id,
                    EventName = eventDetails.Name,
                    SeatNumber = ticket.SeatNumber,
                    EventDate = eventDetails.Date,
                    Location = eventDetails.Location,
                    Price = eventDetails.Price,
                });
            }

            return View(purchasedTickets);
        }

    }
}
