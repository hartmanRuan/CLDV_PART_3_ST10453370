using System.Reflection.Metadata;
using _10453370_POE_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace _10453370_POE_WebApp.Controllers
{
    public class VenueController : Controller
    {
        private readonly EventEaseDBContext _context;
        

        public VenueController(EventEaseDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var venues = await _context.Venue.ToListAsync();
            return View(venues);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                
                if (venue.ImageFile != null)
                {
                    var blobUrl = await UploadImageToBlobAsync(venue.ImageFile);
                    venue.ImageURL = blobUrl;
                }



                _context.Add(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessC Message"] = "Venue created successfully";
                return RedirectToAction(nameof(Index));
            }


            foreach (var kvp in ModelState)
            {
                foreach (var error in kvp.Value.Errors)
                {
                    Console.WriteLine($"Model error for {kvp.Key}: {error.ErrorMessage}");
                }
            }

            return View(venue);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var venues = await _context.Venue.FirstOrDefaultAsync(m => m.Venue_ID == id);
            if (venues == null)
            {
                return NotFound();
            }
            return View(venues);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var venues = await _context.Venue.FirstOrDefaultAsync(m => m.Venue_ID == id);

            if (venues == null)
            {
                return NotFound();
            }
            return View(venues);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venues = await _context.Venue.FindAsync(id);
            if (venues == null) return NotFound();

            var hasBookings = await _context.Booking.AnyAsync(b => b.Venue_ID == id);
            if (hasBookings)
            {
                TempData["Error Message"] = "Cannot delete this venue because it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Venue.Remove(venues);
            await _context.SaveChangesAsync();
            TempData["Success Message"] = "Venue deleted successfully";
            return RedirectToAction(nameof(Index));
        }


        private bool EventExists(int id)
        {
            return _context.Venue.Any(e => e.Venue_ID == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venues = await _context.Venue.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            return View(venues);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venues)
        {
            if (id != venues.Venue_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                try
                {
                        if (venues.ImageFile != null)
                        {
                            
                            var blobUrl = await UploadImageToBlobAsync(venues.ImageFile);
                            venues.ImageURL = blobUrl;
                        }
                        else
                        {
                            
                        }

                        _context.Update(venues);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Venue updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(venues.Venue_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venues);
        }

        private async Task<string> UploadImageToBlobAsync(IFormFile imageFile)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=10453370storageaccount;AccountKey=4Z7MO1WUY7g0eNF4RqqtG9NGKs3tQEH4NtoqhTUaB0kYo6OUfLRdTGGXJpTOdAIFdJpRIfxS+1sT+AStoKo6UQ==;EndpointSuffix=core.windows.net";
            var containerName = "cldv6211container";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(imageFile.FileName));

            var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = imageFile.ContentType
            };

            using (var stream = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
            }

            return blobClient.Uri.ToString();
        }
    }
}
