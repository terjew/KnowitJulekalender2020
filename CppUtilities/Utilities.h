#pragma once

#include <vector>
#include <chrono>
#include <functional>

int measure(const std::function<int()>& f)
{
  int result;
  auto t1 = std::chrono::high_resolution_clock::now();
  int runs = 100;
  for (int i = 0; i < runs; i++) {
    result = f();
  }
  auto t2 = std::chrono::high_resolution_clock::now();
  auto totalus = std::chrono::duration_cast<std::chrono::microseconds>(t2 - t1).count();
  auto perIteration = totalus / (double)runs;
  std::cout << "runtime: " << totalus << "us, " << perIteration  << " us per run" << std::endl;
  return result;
}

std::vector<bool> SievePrimes(int maxPrime)
{
  std::vector<bool> primes = std::vector<bool>(maxPrime, true);
  int maxSquared = (int)sqrt(maxPrime);
  primes[0] = false;
  primes[1] = false;
  for (int j = 4; j < maxPrime; j += 2)
  {
    primes[j] = false;
  }

  for (long i = 3; i < maxSquared; i += 2)
  {
    if (primes[i])
    {
      long j = i * i;
      while (j < maxPrime)
      {
        primes[j] = false;
        j += i;
      }
    }
  }
  return primes;
}
