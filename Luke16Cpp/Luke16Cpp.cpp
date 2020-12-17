#include <iostream>
#include <math.h>
#include <numeric>
#include <execution>
#include <functional>
#include "../CppUtilities/Utilities.h"


inline bool IsPerfectSquareOptimized(int n)
{
  if ((0x202021202030213 & (1L << (int)(n & 63))) > 0)
  {
    long t = (long)round(sqrt((double)n));
    return t * t == n;
  }
  return false;
}

int CountSquareAbundantParallel(int max)
{
  std::vector<int> sumDivisors(max, 1);
  sumDivisors[0] = 0;
  for (int divisor = 2; divisor < max; divisor++)
  {
    for (int number = divisor * 2; number < max; number += divisor)
    {
      sumDivisors[number] += divisor;
    }
    sumDivisors[divisor]++;
  }

  std::vector<int> isSquareAbundant(max);

  std::transform(std::execution::par, sumDivisors.cbegin(), sumDivisors.cend(), isSquareAbundant.begin(), [&](const int& v) -> int {
    size_t i = &v - &sumDivisors[0];
    auto diff = v - i;
    return (diff > 0 && IsPerfectSquareOptimized(diff));
  });

  int count = std::reduce(std::execution::par, isSquareAbundant.cbegin(), isSquareAbundant.cend());
  return count;
}

int CountSquareAbundant(int max)
{
  std::vector<int> sumDivisors(max, 1);
  for (int divisor = 2; divisor < max; divisor++)
  {
    for (int number = divisor * 2; number < max; number += divisor)
    {
      sumDivisors[number] += divisor;
    }
    sumDivisors[divisor]++;
  }


  int count = 0;
  for (int i = 2; i < max; i++)
  {
    auto diff = sumDivisors[i] - i;
    if (diff > 0 && IsPerfectSquareOptimized(diff)) count++;
  }

  return count;
}

int main()
{  
  int numElements = MeasurePerformance("CountSquareAbundantParallel", 10, [&]() {
    return CountSquareAbundantParallel(1000000);
  });
  std::cout << numElements << std::endl;

  numElements = MeasurePerformance("CountSquareAbundant", 10, [&]() {
    return CountSquareAbundant(1000000);
  });
  std::cout << numElements << std::endl;

  int count = MeasurePerformance("IsPerfectSquareOptimized", 100, [&]() {
    int count = 0;
    for (int i = 0; i < 1000000; i++) if (IsPerfectSquareOptimized(i)) count++;
    return count;
  });

  std::cout << count << std::endl;
}
