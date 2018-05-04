# Kubernetes Probe Demo

### How to create image locally

```batch
# Build container image
docker build . -t kubernetesprobedemo:v1

# Run container
kubectl apply -f KubernetesProbeDemo.yaml --record
``` 