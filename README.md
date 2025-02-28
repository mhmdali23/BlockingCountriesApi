# Blocked Countries API

A .NET Core Web API to manage blocked countries and validate IP addresses using third-party geolocation services (such as [ipapi.co](https://ipapi.co/documentation) or [IPGeolocation.io](https://ipgeolocation.io/documentation.html)). This API leverages in-memory data storage to maintain the blocked countries list and log failed access attempts, eliminating the need for a database.

## Table of Contents

- [Overview](#overview)
- [Features & Endpoints](#features--endpoints)
- [Technology Stack](#technology-stack)

## Overview

This project provides a RESTful API that allows clients to:
- Add and remove blocked countries.
- Retrieve a paginated and searchable list of blocked countries.
- Lookup country details for a given IP address.
- Check if a request’s originating IP is blocked.
- Log failed blocked access attempts.
- Temporarily block a country for a specified duration with automatic unblocking via a background service.

The API uses third-party geolocation APIs for IP lookups and demonstrates best practices in thread-safe in-memory data management using .NET Core.

## Features & Endpoints

1. **Add a Blocked Country**
   - **Endpoint:** `POST /api/countries/block`
   - **Input:** Country code (e.g., `"US"`, `"GB"`, `"EG"`)
   - **Behavior:** Adds the country to an in-memory blocked list; duplicates are not allowed.

2. **Delete a Blocked Country**
   - **Endpoint:** `DELETE /api/countries/block/{countryCode}`
   - **Behavior:** Removes the specified country from the blocked list.
   - **Error Handling:** Returns a 404 error if the country is not found in the blocked list.

3. **Get All Blocked Countries**
   - **Endpoint:** `GET /api/countries/blocked`
   - **Features:** 
     - **Pagination:** Supports `page` and `pageSize` query parameters.
     - **Search/Filter:** Allows filtering by country code or name (data stored in-memory).

4. **Find My Country via IP Lookup**
   - **Endpoint:** `GET /api/ip/lookup?ipAddress={ip}`
   - **Behavior:** Calls a third-party geolocation API to retrieve country details (e.g., country code, name, ISP).
   - **Validation:** Ensures the IP address format is valid. If omitted, the API uses the caller's IP via `HttpContext`.

5. **Verify If IP is Blocked**
   - **Endpoint:** `GET /api/ip/check-block`
   - **Process:**
     1. Automatically retrieves the caller's external IP address.
     2. Looks up the country code using the third-party geolocation API.
     3. Checks if the retrieved country is in the blocked list.
     4. Logs the attempt accordingly.

6. **Log Failed Blocked Attempts**
   - **Endpoint:** `GET /api/logs/blocked-attempts`
   - **Features:** 
     - Returns a paginated list of blocked attempts.
     - Each log entry includes details such as IP address, timestamp, country code, blocked status, and user agent.

7. **Temporarily Block a Country**
   - **Endpoint:** `POST /api/countries/temporal-block`
   - **Request Body Example:**
     ```json
     {
       "countryCode": "EG",
       "durationMinutes": 120
     }
     ```
   - **Behavior:** Blocks a country for the specified duration (between 1 and 1440 minutes). 
   - **Validation:** 
     - Ensures `durationMinutes` is within the allowed range.
     - Validates the country code.
     - Prevents duplicate temporal blocks (returns a 409 Conflict if already blocked).
   - **Background Service:** A service runs every 5 minutes to remove expired temporary blocks.

## Technology Stack

- **.NET Core Web API:** Developed using .NET Core (version 7/8/9).
- **In-Memory Storage:** Utilizes thread-safe collections such as `ConcurrentDictionary` and in-memory lists.
- **HTTP Client:** Uses `Microsoft.Extensions.Http` for integrating with third-party geolocation APIs.
- **JSON Parsing:** Optionally uses `Newtonsoft.Json` for JSON serialization/deserialization.
- **Swagger:** Integrated for API documentation and testing.
