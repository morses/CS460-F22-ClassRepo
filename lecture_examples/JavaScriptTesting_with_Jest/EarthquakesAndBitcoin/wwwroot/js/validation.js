/*
 * Simple object validation.  Can check that objects have properties and also their values.
 * There are lots of JS libraries that will do this and much more, but plain JS works to get started.
 * */

// Expects a schema like this of what properties the object has
const schemaExample = {
    magnitude: x => typeof x === 'number' && x >= 0,                // check type and value
    location: x => typeof x === 'string' && /[A-Za-z0-9]*/.test(x), // can test by regex
    eTime: x => typeof x === 'number'
};

// Thanks to Patrick Roberts answer: https://stackoverflow.com/questions/38616612/javascript-elegant-way-to-check-object-has-required-properties
// Pass in the object to validate properties on, and the schema that defines what properties should exist and a validation function to run.
// Returns an array that is empty(passes validation), or contains Error object corresponding to property names that failed validation
const validateObject = (obj, schema) => Object
    .keys(schema)
    .filter(key => !schema[key](obj[key]))
    .map(key => new Error(`${key}`));

// Apply object validation to every object in this array.  Returns count of errors.  Could easily extend to returning where the errors are.
// Pass in an array of objects plus a schema,
// Returns the number of validation errors found
const validateArrayOfObjects = (arr, schema) => arr
    .map(obj => validateObject(obj, schema))
    .reduce((acc, arr) => arr.length > 0 ? acc + 1 : 0, 0);  // use a fold to count up the number of entries whose length is > 0


/* Client side validation of data from USGS 
 * 
 * */
function validateEarthquakeData(data) {
    if (data === undefined || !Array.isArray(data))
        return false;
    const schema = {
        magnitude: x => typeof x === 'number' && x >= 0,
        location: x => typeof x === 'string',
        eTime: x => typeof x === 'number'
    };

    return validateArrayOfObjects(data, schema) === 0;
}

/* Client side validation of data from CryptoWatch. 
 * 
 * */
function validateBitcoinData(data) {
    if (data === undefined || !Array.isArray(data))
        return false;
    // only validate the ones we're using
    const schema = {
        closeTime: x => typeof x === 'number',  // could also check values
        lowPrice: x => typeof x === 'number',
        highPrice: x => typeof x === 'number',
        closePrice: x => typeof x === 'number'
    };

    return validateArrayOfObjects(data, schema) === 0;
}


export { validateObject, validateArrayOfObjects, validateEarthquakeData, validateBitcoinData }