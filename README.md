# Kubernetes Probe Demo

## Build Status

[![Build Status](https://dev.azure.com/jannemattila/jannemattila/_apis/build/status/JanneMattila.KubernetesProbeDemo?branchName=master)](https://dev.azure.com/jannemattila/jannemattila/_build/latest?definitionId=54&branchName=master)
![Docker Pulls](https://img.shields.io/docker/pulls/jannemattila/k8s-probe-demo?style=plastic)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Introduction

Originally this was created for this blog post about
[Playing with Kubernetes livenessProbe and readinessProbe probes](https://docs.microsoft.com/en-us/archive/blogs/jannemattila/playing-with-kubernetes-livenessprobe-and-readinessprobe-probes),
however this has been proven to be pretty handy tool for
various health check type of [tests](https://github.com/JanneMattila/some-questions-and-some-answers/blob/master/q%26a/app_service_and_health_check.md) as well.

You can easily deploy this app to Kubernetes or
PaaS like services such as Azure App Service and test how the platform
behaves under various conditions. Hopefully, it helps to step up
your own availability game.

### How to create image locally

```batch
# Build container image
docker build . -t kubernetesprobedemo:v1

# Run container
kubectl apply -f KubernetesProbeDemo.yaml --record
``` 