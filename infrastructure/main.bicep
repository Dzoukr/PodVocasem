param commithash string

@secure()
param storageaccount string

@secure()
param spotifyclientid string

@secure()
param spotifyclientsecret string

@description('Resource group location')
param location string = resourceGroup().location

@description('Specifies the name of the spotify checker job.')
param logAnalyticsName string = 'podvocasem-logs'

// Web app
@description('Specifies the name of the web app.')
param webAppName string = 'podvocasem-web'

@description('Specifies the docker container image to deploy.')
param webAppImage string = 'dzoukr/podvocasem:${commithash}'

// Jobs
@description('Specifies the name of the spotify checker job.')
param jobAppName string = 'podvocasem-spotify-checker'

@description('Specifies the docker container image to deploy.')
param jobAppImage string = 'dzoukr/podvocasem-spotify-checker:${commithash}'

@description('Specifies the name of the container app environment.')
// param containerAppEnvName string = 'env-${uniqueString(resourceGroup().id)}'
param containerAppEnvName string = 'podvocasem-env'

@description('Number of CPU cores the container can use. Can be with a maximum of two decimals.')
@allowed([
    '0.25'
    '0.5'
    '0.75'
    '1'
    '1.25'
    '1.5'
    '1.75'
    '2'
])
param cpuCore string = '0.5'

@description('Amount of memory (in gibibytes, GiB) allocated to the container up to 4GiB. Can be with a maximum of two decimals. Ratio with CPU cores must be equal to 2.')
@allowed([
    '0.5'
    '1'
    '1.5'
    '2'
    '3'
    '3.5'
    '4'
])
param memorySize string = '1'

@description('Minimum number of replicas that will be deployed')
@minValue(0)
@maxValue(25)
param minReplicas int = 0

@description('Maximum number of replicas that will be deployed')
@minValue(0)
@maxValue(25)
param maxReplicas int = 3

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
    name: logAnalyticsName
    location: location
    properties: {
        sku: {
            name: 'PerGB2018'
        }
    }
}

resource containerAppEnv 'Microsoft.App/managedEnvironments@2023-05-01' = {
    name: containerAppEnvName
    location: location
    //   sku: {
    //     name: 'Consumption'
    //   }
    properties: {
        appLogsConfiguration: {
            destination: 'log-analytics'
            logAnalyticsConfiguration: {
                customerId: logAnalytics.properties.customerId
                sharedKey: logAnalytics.listKeys().primarySharedKey
            }
        }
    }
}

resource webApp 'Microsoft.App/containerApps@2023-05-01' = {
    name: webAppName
    location: location
    properties: {
        environmentId: containerAppEnv.id
        configuration: {
            ingress: {
                external: true
                targetPort: 80
                allowInsecure: false
                traffic: [
                    {
                        latestRevision: true
                        weight: 100
                    }
                ]
            }
            secrets: [
                {
                    name: 'storageaccount'
                    value: storageaccount
                }
            ]
        }
        template: {
            // revisionSuffix: 'firstrevision'

            containers: [
                {
                    name: webAppName
                    image: webAppImage
                    resources: {
                        cpu: json(cpuCore)
                        memory: '${memorySize}Gi'
                    }
                    env: [
                        {
                            name: 'StorageAccount'
                            secretRef: 'storageaccount'
                        }
                    ]
                }
            ]
            scale: {
                minReplicas: minReplicas
                maxReplicas: maxReplicas
            }
        }
    }
}

resource spotifyChecker 'Microsoft.App/jobs@2023-05-01' = {
    name: jobAppName
    location: location
    properties: {
        environmentId: containerAppEnv.id
        
        configuration: {
            replicaTimeout: 1800
            triggerType: 'Schedule'
            scheduleTriggerConfig: {
                cronExpression: '0 * * * *'
                replicaCompletionCount: 1
            }
            
            secrets: [
                {
                    name: 'storageaccount'
                    value: storageaccount
                }
                {
                    name: 'spotifyclientid'
                    value: spotifyclientid
                }
                {
                    name: 'spotifyclientsecret'
                    value: spotifyclientsecret
                }
            ]
        }
        template: {
            // revisionSuffix: 'firstrevision'

            containers: [
                {
                    name: jobAppName
                    image: jobAppImage
                    resources: {
                        cpu: json(cpuCore)
                        memory: '${memorySize}Gi'
                    }
                    env: [
                        {
                            name: 'StorageAccount'
                            secretRef: 'storageaccount'
                        }
                        {
                            name: 'SpotifyClientId'
                            secretRef: 'spotifyclientid'
                        }
                        {
                            name: 'SpotifyClientSecret'
                            secretRef: 'spotifyclientsecret'
                        }
                    ]
                }
            ]
        }
    }
}
