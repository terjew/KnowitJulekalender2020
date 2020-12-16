#include <iostream>
#include <math.h>
#include <memory.h>
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

int CountSquareAbundant(int max)
{
  int* sumDivisors = new int[max];
  memset(sumDivisors, 0, sizeof(int) * max);
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

  delete[] sumDivisors;
  return count;
}

int main()
{  
  int numElements = MeasurePerformance("CountSquareAbundant", 10, [&]() {
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
