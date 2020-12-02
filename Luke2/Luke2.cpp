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

int FindLowerPrime(int num, std::vector<bool> &primes)
{
  while (num > 0) {
    if (primes[num]) return num;
    num--;
  }
  return 0;
}

int CountPackagesDelivered(int max)
{
  auto primes = SievePrimes(max);
  int count = 0;
  for (int i = 0; i < max; i++)
  {
    if (HasDigit7(i)) {
      i += FindLowerPrime(i, primes);
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
  auto func = [&]() {return CountPackagesDelivered(max); };
  int count = measure(func);
  std::cout << "\nNissen leverer " << count << " pakker" << std::endl;
}
