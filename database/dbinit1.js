db = db.getSiblingDB('TrainSystem');

db.createCollection("TrainStops");

db.TrainStops.insertMany([
    {
        name: "Main train station",
        city: "Cracov"
    },
    {
        name: "Plaszow",
        city: "Cracov"
    },
    {
        name: "Main train station",
        city: "Warsaw"
    },
    {
        name: "West train station",
        city: "Rzeszow"
    }]
);
