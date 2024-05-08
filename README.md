# MiApiConSerilog


Ejemplo Deployment

    apiVersion: apps/v1
    kind: Deployment
    metadata:
      labels:
        app: web
      name: web
      namespace: default
    spec:
      progressDeadlineSeconds: 600
      replicas: 1
      revisionHistoryLimit: 10
      selector:
        matchLabels:
          app: web
      strategy:
        rollingUpdate:
          maxSurge: 25%
          maxUnavailable: 25%
        type: RollingUpdate
      template:
        metadata:
          creationTimestamp: null
          labels:
            app: web
        spec:
          containers:
          - env:
            - name: INSTANA_AGENT_HOST
              valueFrom:
                fieldRef:
                  apiVersion: v1
                  fieldPath: status.hostIP
            - name: OTEL_EXPORTER_OTLP_ENDPOINT
              value: http://$(INSTANA_AGENT_HOST):4317
            - name: POD_NAME
              valueFrom:
                fieldRef:
                  apiVersion: v1
                  fieldPath: metadata.name
            - name: POD_UID
              valueFrom:
                fieldRef:
                  apiVersion: v1
                  fieldPath: metadata.uid
            - name: OTEL_RESOURCE_ATTRIBUTES
              value: service.instance.id=$(POD_NAME),k8s.pod.uid=$(POD_UID)
            image: juanconde24/miapi-con-serilog-alpine:v4
            imagePullPolicy: Always
            name: nginx
            resources: {}
            terminationMessagePath: /dev/termination-log
            terminationMessagePolicy: File
          dnsPolicy: ClusterFirst
          restartPolicy: Always
          schedulerName: default-scheduler
          securityContext: {}
          terminationGracePeriodSeconds: 30
