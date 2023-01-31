using BenchmarkDotNet.Running;
using Dopamine.BenchmarkDotNet.BenchShaders;
using Dopamine.BenchmarkDotNet.BenchFluidSimulation;
using Dopamine.BenchmarkDotNet.BenchRenderers;

// How Benchmark.net works
// https://www.youtube.com/watch?v=mmza9x3QxYE&ab_channel=IAmTimCorey

BenchmarkRunner.Run<BenchCPUvsGPURenderer>();
//BenchmarkRunner.Run<BenchFluidDiffuseCUDA>();
//BenchmarkRunner.Run<BenchShaderVsNatifPicelRendering>();