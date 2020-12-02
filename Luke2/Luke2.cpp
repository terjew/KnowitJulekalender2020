#include <iostream>
#include <vector>
#include "..\CppUtilities\Utilities.h"

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

int FindLowerPrime(int num)
{
  while (num > 0) {
    if (IsPrime(num)) return num;
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
  int numElements = MeasurePerformance("Sieve (ikke i bruk)", [&]() {
    return (int)SievePrimes(max).size();
  });

  int numPrimes = MeasurePerformance("GetAllPrimes (ikke i bruk)", [&]() {
    return (int)GetAllPrimes(max).size();
  });

  int count = MeasurePerformance("Telle pakker uten sieve", [&]() {
    return CountPackagesDelivered(max); 
  });
  std::cout << "\nNissen leverer " << count << " pakker" << std::endl;
}
