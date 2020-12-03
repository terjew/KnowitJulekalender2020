#include <iostream>
#include <vector>
#include "..\CppUtilities\Utilities.h"
#include "..\CppUtilities\Miller-Rabin.h"

bool HasDigit7(int i)
{
  int digit = 0;
  int rest = i;
  while (rest > 0)
  {
    digit = rest % 10;
    if (digit == 7) return true;
    rest /= 10;
  }
  return false;
}

std::function<bool(int)> primeTestFunc;

int FindLowerPrime(int num)
{
  while (num > 0) {
    if (primeTestFunc(num)) return num;
    num--;
  }
  return 0;
}

int CountPackagesDelivered(int max)
{
  int count = 0;
  for (int i = 0; i < max; i++)
  {
    if (HasDigit7(i)) {
      i += FindLowerPrime(i);
    }
    else {
      count++;
    }
  }
  return count;
}

int main()
{
  int max = 5433000;
  int numElements = MeasurePerformance("Telle pakker med sieve (ikke i bruk)", 100, [&]() {
    return (int)SievePrimes(max).size();
  });

  int numPrimes = MeasurePerformance("GetAllPrimes (ikke i bruk)", 100, [&]() {
    return (int)GetAllPrimes(max).size();
  });

  primeTestFunc = [&](int n) { return IsPrime(n); };
  int count = MeasurePerformance("Telle pakker uten sieve", 1000, [&]() {
    return CountPackagesDelivered(max);
  });

  primeTestFunc = [&](int n) { return millerRabin::isprime(n, 5); };
  MeasurePerformance("Telle pakker uten sieve, Miller-Rabin (5)", 1000, [&]() {
    return CountPackagesDelivered(max);
  });

  primeTestFunc = [&](int n) { return millerRabin::isprime(n, 13); };
  MeasurePerformance("Telle pakker uten sieve, Miller-Rabin (13)", 1000, [&]() {
    return CountPackagesDelivered(max);
  });

  std::cout << "\nNissen leverer " << count << " pakker" << std::endl;
}
