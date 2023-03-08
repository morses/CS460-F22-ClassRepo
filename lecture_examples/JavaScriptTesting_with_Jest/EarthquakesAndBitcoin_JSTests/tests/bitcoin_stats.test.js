import { bitcoinStats } from '../../EarthquakesAndBitcoin/wwwroot/js/statistics.js';
import { validateObject } from '../../EarthquakesAndBitcoin/wwwroot/js/validation.js';

/** 
    Showing how we'd set this up for TDD: write the function interface first (bitcoinStats),
    then write these tests, then implement bitcoinStats to pass tests
 */


// Use a seeded random number generator to create predictable random numbers for testing
let seedrandom = require('seedrandom');

// this one isn't realistic, purely random about a mean
function makeRandomBitcoinData(seed, size) {
    let rng = seedrandom(seed);
    const startTime = Math.floor(1e9 * (rng() + 1));
    const startPrice = Math.floor(1e4 * (rng() + 1));
    return {
        startTime: startTime,
        startPrice: startPrice,
        data: Array.from({ length: size }, (_, i) => ({ closeTime: startTime + i * 300, closePrice: startPrice + 50 * (rng() - 0.5), volume: rng() * 10 }))
    }
}

function makeRisingBitcoinData(seed,size,start,end) {
    // ramps from low to high so has known min, max and average
    return undefined;
}

function makeFallingBitcoinData(seed, size, start, end) {
    // ramps from high to low so has known min, max and average
    return undefined;
}

function makeRealisticBitcoinData(seed, size, start, end) {
    // fluctuates up and down but has known min, max and best trade data
    return undefined;
}

describe('Statistics calculations on Bitcoin data from CryptoWatch tests', () => {

    test('stats contain required parameters', () => {
        // Arrange
        const schema = {
            closePriceAve: x => typeof x === 'number',
            volumeAve: x => typeof x === 'number',
            minClosePrice: x => typeof x === 'number',
            maxClosePrice: x => typeof x === 'number',
            bestTrade: x => typeof x === 'object' && 'buyTime' in x && 'sellTime' in x && 'priceDifference' in x
        };
        
        const data = makeRandomBitcoinData('KJHKL67898HUY90', 100).data;

        // Act
        const stats = bitcoinStats(data);

        //Assert
        expect(validateObject(stats,schema)).toBe([]);
    });

    // Test averages are close to the known averages above, with the random data
    test('closing price is correct', () => {
        // Arrange
        const out = makeRandomBitcoinData('KJHKL67898HUY90', 100);

        // Act
        const stats = bitcoinStats(out.data);

        //Assert
        expect(Math.abs(stats.closePriceAve - out.startPrice)/out.startPrice).toBeLessThan(0.01);   // average to within 1%
    });

    // Test values for Rising data

    // Test values for Falling data

    // Test values for realistic data, including best trade

});