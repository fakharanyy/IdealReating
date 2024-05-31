const { MongoClient } = require('mongodb');

const uri = 'mongodb://localhost:27017';

const dbName = 'PersonDb';
const collectionName = 'Persons';

const data = [
    {
        "name": "Ahmed Mohammed",
        "telephoneNumber": "20-010334445",
        "address": "10 Road Street",
        "country": "Egypt"
    },
    {
        "name": "Mona Mahmoud",
        "telephoneNumber": "20-010334445",
        "address": "11 Road Street",
        "country": "Egypt"
    },
    {
        "name": "Mohammed Rabie",
        "telephoneNumber": "970-111111111",
        "address": "15 Road Street",
        "country": "Palestine"
    },
    {
        "name": "Ana Yousif",
        "telephoneNumber": "961-111111111",
        "address": "20 Road Street",
        "country": "Lebanon"
    }
];

async function initDatabase() {
    const client = new MongoClient(uri);

    try {
        await client.connect();

        const database = client.db(dbName);

        await database.createCollection(collectionName);

        await database.collection(collectionName).insertMany(data);

        console.log('Database initialized successfully!');
    } catch (err) {
        console.error('Error initializing database:', err);
    } finally {
        await client.close();
    }
}

initDatabase();
