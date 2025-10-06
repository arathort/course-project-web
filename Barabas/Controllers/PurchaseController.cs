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

            var tickets = await _ticketService.GetAvailableTickets(eventId);
            var available = tickets.Where(t => t.IsActive).ToList();
            var taken = tickets.Where(t => !t.IsActive).ToList();

            var model = new TicketSelectionViewModel
            {
                EventName = ev.Name,
                AllSeats = tickets.Select(t => t.SeatNumber).ToList(),
                AvailableSeats = available.Select(t => t.SeatNumber).ToList(),
                AvailableTicketIds = available.Select(t => t.Id).ToList(),
                TakenSeats = taken.Select(t => t.SeatNumber).ToList()
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
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userOrders = await _orderService.GetOrdersByUserId(userId);
            var purchasedTickets = new List<PurchasedTicketViewModel>();

            foreach (var order in userOrders)
            {
                var ticket = await _ticketService.GetTicketById(order.TicketId);
                if (ticket == null)
                {
                    Console.WriteLine($"⚠️ Ticket not found for TicketId = {order.TicketId}");
                    continue;
                }

                var eventDetails = await _eventService.GetEventById(ticket.EventId);
                if (eventDetails == null)
                {
                    Console.WriteLine($"⚠️ Event not found for EventId = {ticket.EventId}");
                    continue;
                }

                purchasedTickets.Add(new PurchasedTicketViewModel
                {
                    TicketId = ticket.Id,
                    EventName = eventDetails.Name,
                    SeatNumber = ticket.SeatNumber,
                    EventDate = eventDetails.Date,
                    Location = eventDetails.Location,
                    Price = eventDetails.Price
                });
            }

            return View(purchasedTickets);
        }


    }
}
