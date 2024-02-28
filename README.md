# Exercise §

Must use:
• ASP.NET CORE MVC (6) •C#
• JavaScript or Typescript • React
Achieve:
Using any public weather API receive data (country, city, temperature, clouds, wind speed) from at least 10 cities in 5 countries
with periodical update 1/min,
store this data in the database
and show the 2 graphs:
● min temperature (Country\City\Temperature\Last update time)
● highest wind speed (Country\City\Wind Speed\Last update time)
● temperature & wind speed trend for last 2 hours on click for both previous graphs
Need to implement following storage procedures:
1. Create a storage procedure that provides information about the max wind speed, country name for a specific country *N name;
    
2. Create a storage procedure that provides information about min temperature, max wind speed, country name
where the min temperature in the country was less than a specific *N temperature.
*N - storage procedure parameter

---

# Summary
This exercise took me about 10 hours to complete. 

## Challenges 
- No knowledge of react - had to learn it along the way. I used to code in Angular in the past, but it is way different.
- Not sure what kind of graph was expected so I used line graph, but it took me a bit to understand how to represent the data.
- Not sure what was the intention of point 1 and 2. I didn't hear about storage procedures. I know stored procedures in MSSQL server, and due to lack of requirements regarding storage in this exercise, I used EF Core with MSSQL as a in memory database, but this type of database does not support stored procedures. If I had to write such procedures and provide a GET API to fetch the wind speed/temperature, it would take me about an hour for both tasks.
- Saved time by skipping unit tests.

## Ideas/comments
- Used OpenWeather as the API, but since the free account is limited to 1000 requests a day, I would consider different weather provider.
- Currently there is a single graph component which renders both graphs. I think it would be better to have a separate component for each graph, or render two graphs using single component but binding to different data(temperature for the first one and windspeed for the second one). I didn't have much time left for the exercise so I left it as it is.
## How to run
Run the app in visual studio. The app should fetch the data every minute and the UI refreshes itself every minute. Give it some time to accumulate data so the graphs are more valuable.
