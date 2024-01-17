import random

file = open("file.sql", "w", encoding="utf-8")

numberOfUsers = 100
numberOfDepartmens = 7
# list of departments must be at least long as "numberOfDepartmens"
departmentList = ["HR", "Administration",
                  "IT", "Accounting", "Deployments", "Marketing", "Management"]
numberOfTeams = 3
numberOfMembers = 4

numberOfMeetings = 250

minYear = 2023
maxYear = 2024
month = 12
day = 28

numberOfWordsInTitleOfMeeting = 3

emptyDescriptionPropability = 7  # between 1 and 10

maxMeetingType = 7
maxMeetingStatus = 3
maxMeetingRepeating = 7

minMeetingsOfMembers = 10
maxMeetingsOfMembers = 20

minMessagesInMeeting = 15
maxMessagesInMeeting = 20
wordsInMessageCount = 2

minAttachmentsInMeeting = 15
maxAttachmentsInMeeting = 20
wordsInNameCount = 2
maxAttachmentsTypes = 9

names = ["Aadam", "Rihanna", "Hollie", "Madeleine", "Carolyn", "Axel", "Ruqayyah", "Jennie", "Shirley", "Keira", "Kelsie", "Levi", "Macie", "Judy", "Neha", "Honey", "Bella", "Vivian", "Charlotte", "Connor", "Tori", "Dawid", "Roosevelt", "Rachel", "Fay", "Francesco", "Mia", "Romeo", "Jamie", "Emil", "Danny", "Nicola", "Abbas", "Savannah", "Kiran", "Daniella", "Felicity", "Brooklyn", "Frederic", "Velma", "Aditya", "Umair", "Scarlet", "Abby", "Melanie", "Edmund", "Halima", "Zane", "Jessie",
         "Amanda", "Maia", "Stephanie", "Donald", "Anoushka", "Dean", "Hugh", "Kamran", "Maddie", "Abi", "Tristan", "Abdur", "Gina", "Kaitlin", "Kyra", "Oliwier", "Amir", "Sidney", "Richie", "Aran", "Aiza", "Samira", "Eugene", "Fintan", "Krish", "Leyla", "Ieuan", "Heath", "Shivam", "Elouise", "Amaya", "Ana", "Ava-Rose", "Cody", "Brett", "Ffion", "Subhaan", "Ahmed", "Eileen", "Moses", "Alina", "Delores", "Iqra", "Kristen", "Josiah", "Megan", "Brooke", "Kieran", "Lily-May", "Lilly-Rose", "Briony"]

surnames = ["Lester", "Nguyen", "Rojas", "Schultz", "Nicholson", "Mcknight", "Malone", "Bauer", "Oneal", "Flynn", "Melendez", "Olsen", "Peters", "Shah", "Galvan", "Lindsay", "Decker", "Craig", "Sanford", "Wallace", "Mitchell", "Chambers", "Horne", "Park", "Giles", "Graham", "Robertson", "Roberts", "Brennan", "Goodman", "Curtis", "Mcclure", "Swanson", "Garner", "Mcdaniel", "Hood", "Alvarez", "Briggs", "Shields", "Petty", "Key", "Chen", "Norris", "Sloan", "Walton", "Yoder", "Nash", "Townsend", "Pham", "Dyer",
            "Ibarra", "Grant", "Vang", "Odonnell", "Wheeler", "Rubio", "Davidson", "Spence", "Oneill", "Ali", "Smith", "Soto", "Ruiz", "Valdez", "Wolf", "Vaughn", "Logan", "Butler", "Willis", "Lopez", "Gordon", "Salinas", "Nichols", "Connor", "Woodard", "Abbott", "Garza", "Mcdonald", "Pacheco", "Fernandez", "Merrill", "Barlow", "Quinn", "Peterson", "Mendoza", "Owens", "Hayden", "Raymond", "Rivers", "Carlson", "Bowers", "Harvey", "Beck", "Shannon", "Burgess", "Moran", "Mcmillan", "Flores", "Stanton", "Atkinson"]


sql = "INSERT INTO users(firstName, lastName, email, password, isAdmin) VALUES "

# password -> pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM= -> "123"

for i in range(numberOfUsers):
    n = names[random.randint(0, len(names) - 1)]
    s = surnames[random.randint(0, len(surnames) - 1)]
    if i != numberOfUsers - 1:
        if i == 0:
            sql += "('Michael', 'Beetle', 'michael.beetle@example.com', 'pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=', '1'), "
        else:
            sql += f"('{n}', '{s}', '{n.lower()}.{s.lower()}@example.com', 'pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=', '0'), "
    else:
        sql += f"('{n}', '{s}', '{n.lower()}.{s.lower()}@example.com', 'pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=', '0')"

sql += ";\n"

file.write(sql)

# ////////////////////////////////////////////////////////////////////////#


directors = random.sample(range(2, numberOfUsers + 1), numberOfDepartmens)

sql = "INSERT INTO departments(name, directorID) VALUES "

for i in range(numberOfDepartmens):
    if i != numberOfDepartmens - 1:
        sql += f"('{departmentList[i]}', {directors[i]}), "
    else:
        sql += f"('{departmentList[i]}', {directors[i]})"

sql += ";\n"
file.write(sql)


allLeader = []
allMembers = []

teamID = 0

for i in range(numberOfDepartmens):
    teams = [
        f"Team_{p+1}_{departmentList[i]}" for p in range(0, numberOfTeams)]
    leaders = random.sample(range(2, numberOfUsers + 1), numberOfTeams)
    for j in range(numberOfTeams):
        while leaders[j] in directors or leaders[j] in allLeader:
            leaders[j] = random.randint(2, numberOfUsers)

    allLeader.append(leaders)
    sql = "INSERT INTO teams(name, departmentID) VALUES "
    for k in range(numberOfTeams):
        if k != numberOfTeams - 1:
            sql += f"('{teams[k]}', {i+1}), "
        else:
            sql += f"('{teams[k]}', {i+1})"
    sql += ";\n"
    file.write(sql)
    # ////////////////////////////////////////////////////////////////////////#

    for k in range(numberOfTeams):
        membersID = random.sample(range(2, numberOfUsers + 1), numberOfMembers)

        for m in range(numberOfMembers):
            while membersID[m] in directors or membersID[m] in allLeader or membersID[m] in allMembers:
                membersID[m] = random.randint(2, numberOfUsers)

        allMembers.append(membersID)

        sql = "INSERT INTO teamsmembers(teamID, memberID, isLeader) VALUES "

        sql += f"({teamID + 1}, {leaders[k]}, 1), "

        for m in range(numberOfMembers):
            if m != numberOfMembers - 1:
                sql += f"({teamID + 1}, {membersID[m]}, 0), "
            else:
                sql += f"({teamID + 1}, {membersID[m]}, 0)"

        sql += ";\n"
        file.write(sql)
        teamID += 1

# ////////////////////////////////////////////////////////////////////////#
lorem = ["Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed iaculis urna ut fermentum auctor. Aliquam dictum suscipit sagittis. Morbi volutpat eleifend metus, a aliquet dolor rhoncus id. Interdum et malesuada fames ac ante ipsum primis in faucibus. Aenean eget leo vitae tellus venenatis pellentesque. Vestibulum vitae odio ut risus consectetur pharetra vitae malesuada metus. Pellentesque non libero eu arcu viverra commodo mollis a tortor.",
         "Ut blandit faucibus sem. Fusce euismod enim non lobortis aliquet. Donec laoreet bibendum imperdiet. Fusce tincidunt lorem vestibulum, interdum felis non, imperdiet ex. Nunc eget auctor turpis. Curabitur nibh lacus, ultrices ut placerat rutrum, sagittis ut justo. Proin lacus nibh, accumsan non est egestas, iaculis luctus leo. Ut accumsan molestie odio, vel consequat lorem aliquam et. Donec euismod ac ligula vitae varius.", "Mauris scelerisque ultricies sem eget blandit. Nam pellentesque porttitor dolor, vitae molestie dui cursus sed. Fusce enim mi, volutpat quis hendrerit sit amet, iaculis vel massa. Fusce quis ante tortor. Donec eu elit sit amet mauris volutpat condimentum. In fermentum vitae felis et vehicula. Curabitur eget urna a ipsum fermentum suscipit eget aliquet enim. Sed vitae ex maximus, accumsan tellus et, pulvinar mauris.", "Nullam odio turpis, scelerisque non quam non, tempor sollicitudin augue. Vestibulum nisi diam, molestie non est ac, tempor pulvinar est. Interdum et malesuada fames ac ante ipsum primis in faucibus. Etiam viverra congue volutpat. Nulla vel rutrum lorem. Aenean dignissim sem nec leo auctor dictum. Praesent sodales neque vitae varius consequat. Suspendisse rutrum vulputate diam, et condimentum nunc.", "Aliquam vulputate, orci nec pulvinar egestas, eros felis pretium metus, euismod elementum nulla mi nec neque. Vestibulum libero sem, venenatis at tellus eget, varius rhoncus metus. Maecenas dapibus ex eu dui sodales finibus. Nunc non libero quis quam bibendum pharetra at at leo. Sed interdum odio a nulla vulputate egestas. Mauris ac ornare velit, nec cursus urna. Ut metus felis, pellentesque eu porta semper, fringilla eu mi."]

ipsum = ['Lorem', 'ipsum', 'dolor', 'sit', 'amet,', 'consectetur', 'adipiscing', 'elit.', 'Sed', 'iaculis', 'urna', 'ut', 'fermentum', 'auctor.', 'Aliquam', 'dictum', 'suscipit', 'sagittis.', 'Morbi', 'volutpat', 'eleifend', 'metus,', 'a', 'aliquet', 'dolor', 'rhoncus', 'id.', 'Interdum', 'et', 'malesuada', 'fames', 'ac', 'ante', 'ipsum', 'primis', 'in', 'faucibus.', 'Aenean', 'eget', 'leo', 'vitae', 'tellus', 'venenatis', 'pellentesque.', 'Vestibulum', 'vitae', 'odio', 'ut', 'risus', 'consectetur', 'pharetra', 'vitae', 'malesuada', 'metus.', 'Pellentesque', 'non', 'libero', 'eu', 'arcu', 'viverra', 'commodo', 'mollis', 'a', 'tortor.', 'Aenean', 'nec', 'fringilla', 'velit,', 'id', 'feugiat', 'sapien.', 'Donec', 'dignissim', 'commodo', 'est', 'et', 'cursus.', 'Vestibulum', 'sit', 'amet', 'aliquet', 'elit.', 'Duis', 'id', 'pulvinar', 'purus,', 'maximus', 'vestibulum', 'risus.', 'Proin', 'dapibus', 'accumsan', 'quam,', 'in', 'rutrum', 'ligula', 'gravida', 'a.', 'Curabitur', 'elementum', 'fringilla', 'scelerisque.', 'Phasellus', 'ut', 'mi', 'sit', 'amet', 'sapien', 'facilisis', 'accumsan.', 'Maecenas', 'in', 'diam', 'nec', 'sapien', 'pharetra', 'mattis', 'id', 'sagittis', 'purus.', 'Ut', 'blandit', 'faucibus', 'sem.', 'Fusce', 'euismod', 'enim', 'non', 'lobortis', 'aliquet.', 'Donec', 'laoreet', 'bibendum', 'imperdiet.', 'Fusce', 'tincidunt', 'lorem', 'vestibulum,', 'interdum', 'felis', 'non,', 'imperdiet', 'ex.', 'Nunc', 'eget', 'auctor', 'turpis.', 'Curabitur', 'nibh', 'lacus,', 'ultrices', 'ut', 'placerat', 'rutrum,', 'sagittis', 'ut', 'justo.', 'Proin', 'lacus', 'nibh,', 'accumsan', 'non', 'est', 'egestas,', 'iaculis', 'luctus', 'leo.', 'Ut', 'accumsan', 'molestie', 'odio,', 'vel', 'consequat', 'lorem', 'aliquam', 'et.', 'Donec', 'euismod', 'ac', 'ligula', 'vitae', 'varius.', 'Etiam', 'vel', 'commodo', 'velit,', 'quis', 'egestas', 'justo.', 'Phasellus', 'sed', 'nulla', 'non', 'nisl', 'mollis', 'porta', 'vel', 'eget', 'augue.', 'Maecenas', 'feugiat', 'rutrum', 'augue', 'ut', 'dictum.', 'Fusce', 'rhoncus', 'enim', 'tortor,', 'sed', 'faucibus', 'mi', 'vulputate', 'vitae.', 'Curabitur', 'euismod', 'magna', 'ut', 'condimentum', 'ullamcorper.', 'Proin', 'tincidunt', 'sapien', 'ex,', 'vitae', 'venenatis', 'tellus', 'lobortis', 'eu.', 'Mauris', 'scelerisque', 'ultricies', 'sem', 'eget', 'blandit.', 'Nam', 'pellentesque', 'porttitor', 'dolor,', 'vitae', 'molestie', 'dui', 'cursus', 'sed.',
         'Fusce', 'enim', 'mi,', 'volutpat', 'quis', 'hendrerit', 'sit', 'amet,', 'iaculis', 'vel', 'massa.', 'Fusce', 'quis', 'ante', 'tortor.', 'Donec', 'eu', 'elit', 'sit', 'amet', 'mauris', 'volutpat', 'condimentum.', 'In', 'fermentum', 'vitae', 'felis', 'et', 'vehicula.', 'Curabitur', 'eget', 'urna', 'a', 'ipsum', 'fermentum', 'suscipit', 'eget', 'aliquet', 'enim.', 'Sed', 'vitae', 'ex', 'maximus,', 'accumsan', 'tellus', 'et,', 'pulvinar', 'mauris.', 'Nullam', 'odio', 'turpis,', 'scelerisque', 'non', 'quam', 'non,', 'tempor', 'sollicitudin', 'augue.', 'Vestibulum', 'nisi', 'diam,', 'molestie', 'non',
         'est', 'ac,', 'tempor', 'pulvinar', 'est.', 'Interdum', 'et', 'malesuada', 'fames', 'ac', 'ante', 'ipsum', 'primis', 'in', 'faucibus.', 'Etiam', 'viverra', 'congue', 'volutpat.', 'Nulla', 'vel', 'rutrum', 'lorem.', 'Aenean', 'dignissim', 'sem', 'nec', 'leo', 'auctor', 'dictum.', 'Praesent', 'sodales', 'neque', 'vitae', 'varius', 'consequat.', 'Suspendisse', 'rutrum', 'vulputate', 'diam,', 'et', 'condimentum', 'nunc.', 'Donec', 'fermentum,', 'arcu',
         'a', 'accumsan', 'posuere,', 'urna', 'nisi', 'maximus', 'nisl,', 'eu', 'cursus', 'nulla', 'quam', 'aliquam', 'massa.', 'Suspendisse', 'potenti.', 'Nam', 'eget', 'blandit', 'felis.', 'Vestibulum', 'viverra', 'elementum', 'felis', 'sed', 'ornare.', 'Aliquam', 'quis', 'lacus', 'sapien.', 'Curabitur', 'vel', 'nisi', 'rutrum', 'dolor', 'maximus', 'faucibus.', 'Quisque', 'eget', 'justo', 'sed', 'libero', 'aliquet', 'condimentum', 'et', 'porta', 'felis.Aliquam', 'vulputate,', 'orci', 'nec', 'pulvinar', 'egestas,', 'eros', 'felis', 'pretium', 'metus,', 'euismod', 'elementum', 'nulla', 'mi', 'nec', 'neque.', 'Vestibulum', 'libero', 'sem,', 'venenatis', 'at', 'tellus', 'eget,', 'varius', 'rhoncus', 'metus.', 'Maecenas', 'dapibus', 'ex', 'eu', 'dui', 'sodales', 'finibus.', 'Nunc', 'non', 'libero', 'quis', 'quam', 'bibendum', 'pharetra', 'at', 'at', 'leo.', 'Sed', 'interdum', 'odio', 'a', 'nulla', 'vulputate', 'egestas.', 'Mauris', 'ac', 'ornare', 'velit,', 'nec', 'cursus', 'urna.', 'Ut', 'metus', 'felis,', 'pellentesque', 'eu', 'porta', 'semper,', 'fringilla', 'eu', 'mi.', 'Suspendisse', 'sed', 'imperdiet', 'arcu.', 'Etiam', 'hendrerit', 'augue', 'lacus,', 'in', 'porta', 'purus', 'porta', 'at.', 'Nulla', 'at', 'neque', 'vitae', 'metus', 'fringilla', 'pellentesque.']


meetingID = 1
for i in range(numberOfMeetings):
    title = ""
    wordsInTitle = random.sample(
        range(0, len(ipsum) - 1), numberOfWordsInTitleOfMeeting)
    for f, j in enumerate(wordsInTitle):
        if f != len(wordsInTitle) - 1:
            if f == 0:
                title += ipsum[j]
                title = title.capitalize()
                title += " "
            else:
                title += ipsum[j]+" "
        else:
            title += ipsum[j]
    date = str(random.randint(minYear, maxYear))+"-" + \
        str(random.randint(1, month))+"-"+str(random.randint(1, day))

    description = lorem[random.randint(
        0, len(lorem) - 1)] if random.randint(1, 10) > emptyDescriptionPropability else "Empty description"

    creatorID = random.randint(1, numberOfUsers)
    typeID = random.randint(1, maxMeetingType)
    statusID = random.randint(1, maxMeetingStatus)
    repID = random.randint(1, maxMeetingRepeating)
    sql = f"INSERT INTO meetings(title, date, description, typeID, statusID, repeatingID) VALUES ('{title}','{date}','{description}',{typeID},{statusID},{repID})"
    sql += ";\n"
    file.write(sql)
    # ////////////////////////////////////////////////////////////////////////#
    numberOfMeetingMembers = random.randint(
        minMeetingsOfMembers, maxMeetingsOfMembers)
    meetingMembers = random.sample(
        range(1, numberOfUsers + 1), numberOfMeetingMembers)
    for k in range(numberOfMeetingMembers):
        while meetingMembers[k] == creatorID:
            meetingMembers[k] = random.randint(1, numberOfUsers)
    sql = "INSERT INTO meetingsmembers(meetingID, memberID, isCreator) VALUES "
    sql += f"({meetingID}, {creatorID}, 1), "
    for j in range(numberOfMeetingMembers):
        if j != numberOfMeetingMembers - 1:
            sql += f"({meetingID}, {meetingMembers[j]}, 0), "
        else:
            sql += f"({meetingID}, {meetingMembers[j]}, 0)"
    sql += ";\n"
    file.write(sql)
    # ////////////////////////////////////////////////////////////////////////#
    numberOfMeetingMessages = random.randint(
        minMessagesInMeeting, maxMessagesInMeeting)
    sql = "INSERT INTO meetingschats(message, meetingID, senderID) VALUES "
    for j in range(numberOfMeetingMessages):
        message = ""
        wordsInMessage = random.sample(
            range(0, len(ipsum) - 1), wordsInMessageCount)
        for f, k in enumerate(wordsInMessage):
            if f != len(wordsInMessage) - 1:
                if f == 0:
                    message += ipsum[j]
                    message = message.capitalize()
                    message += " "
                else:
                    message += ipsum[j]+" "
            else:
                message += ipsum[j]
        if j != numberOfMeetingMessages - 1:
            sql += f"('{message}', {meetingID}, {meetingMembers[random.randint(0, len(meetingMembers) - 1)]}), "
        else:
            sql += f"('{message}', {meetingID}, {meetingMembers[random.randint(0, len(meetingMembers) - 1)]})"
    sql += ";\n"
    file.write(sql)

    # ////////////////////////////////////////////////////////////////////////#
    numberOfMeetingAttachments = random.randint(
        minAttachmentsInMeeting, maxAttachmentsInMeeting)
    sql = "INSERT INTO meetingsattachments(name, typeID, meetingID, senderID) VALUES "
    for j in range(numberOfMeetingAttachments):
        name = ""
        wordsInName = random.sample(range(0, len(ipsum) - 1), wordsInNameCount)
        for f, k in enumerate(wordsInName):
            if f != len(wordsInName) - 1:
                if f == 0:
                    name += ipsum[j]
                    name = name.capitalize()
                    name += " "
                else:
                    name += ipsum[j]+" "
            else:
                name += ipsum[j]
        typeID = random.randint(1, maxAttachmentsTypes)
        if j != numberOfMeetingAttachments - 1:
            sql += f"('{name}', {typeID}, {meetingID}, {meetingMembers[random.randint(0, len(meetingMembers) - 1)]}), "
        else:
            sql += f"('{name}', {typeID}, {meetingID}, {meetingMembers[random.randint(0, len(meetingMembers) - 1)]})"
    sql += ";\n"
    file.write(sql)
    meetingID += 1

file.close()
