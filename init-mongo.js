db.createUser({
    user: 'admin',
    pwd: 'Test123!',
    roles: [
        {
            role: 'readWrite',
            db: 'UserDB',
        },
    ],
});

db = new Mongo().getDB('UserDB');

db.createCollection('users', { capped: false });

db.users.insertMany([
    {
        name: 'Ahmed Mohammed',
        telephoneNumber: '20-010334445',
        address: '10 Road Street',
        country: 'Egypt',
    },
    {
        name: 'Mona Mahmoud',
        telephoneNumber: '20-010334445',
        address: '11 Road Street',
        country: 'Egypt',
    },
    {
        name: 'Mohammed Rabie',
        telephoneNumber: '970-111111111',
        address: '15 Road Street',
        country: 'Palestine',
    },
    {
        name: 'Ana Yousif',
        telephoneNumber: '961-111111111',
        address: '20 Road Street',
        country: 'Lebanon',
    },
]);
