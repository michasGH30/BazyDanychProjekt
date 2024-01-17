string = "Aadam Lester Rihanna Nguyen Hollie Rojas Madeleine Schultz Carolyn Nicholson Axel Mcknight Ruqayyah Malone Jennie Bauer Shirley Oneal Keira Flynn Kelsie Melendez Levi Olsen Macie Peters Judy Shah Neha Galvan Honey Lindsay Bella Decker Vivian Craig Charlotte Sanford Connor Wallace Tori Mitchell Dawid Chambers Roosevelt Horne Rachel Park Fay Giles Francesco Graham Mia Robertson Romeo Roberts Jamie Brennan Emil Goodman Danny Curtis Nicola Mcclure Abbas Swanson Savannah Garner Kiran Mcdaniel Daniella Hood Felicity Alvarez Brooklyn Briggs Frederic Shields Velma Petty Aditya Key Umair Chen Scarlet Norris Abby Sloan Melanie Walton Edmund Yoder Halima Nash Zane Townsend Jessie Pham Amanda Dyer Maia Ibarra Stephanie Grant Donald Vang Anoushka Odonnell Dean Wheeler Hugh Rubio Kamran Davidson Maddie Spence Abi Oneill Tristan Ali Abdur Smith Gina Soto Kaitlin Ruiz Kyra Valdez Oliwier Wolf Amir Vaughn Sidney Logan Richie Butler Aran Willis Aiza Lopez Samira Gordon Eugene Salinas Fintan Nichols Krish O'Connor Leyla Woodard Ieuan Abbott Heath Garza Shivam Mcdonald Elouise Pacheco Amaya Fernandez Ana Merrill Ava-Rose Barlow Cody O'Quinn Brett Peterson Ffion Mendoza Subhaan Owens Ahmed Hayden Eileen Raymond Moses Rivers Alina Carlson Delores Bowers Iqra Harvey Kristen Beck Josiah Shannon Megan Burgess Brooke Moran Kieran Mcmillan Lily-May Flores Lilly-Rose Stanton Briony Atkinson"

splited = string.split(" ")

names = [splited[i] for i in range(len(splited)) if i % 2 == 0]
surnames = [splited[i] for i in range(len(splited)) if i % 2 != 0]

print(names)
print(surnames)
