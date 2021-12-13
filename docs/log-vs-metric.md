# Logs vs Metrics

## Logs

13:00:00.000 INFO Handling GET /path?query=value1

14:00:00.000 INFO Handling POST /path?query=value2

15:00:00.000 INFO Handling GET /path?query=value3

## Metrics

| Metric | Timestamp | Value |
| :---: | :---: | :---: |
| GET | 13:01 | 1 |
| POST | 13:01 | 0 |
| GET | 15:01 | 2 |
| POST | 15:01 | 1 |

## Comparison

|  | Metrics | Logs |
| :---: | :---: | :---: |
| Data | Values (unit) | Text |
| Agregation | Easier | Harder |
| Storage Requirments | Lower | Higher |
| Post processing | Easier | Harder |
| Insight | Shallower | Deeper |
| Main purpose |Anomaly detection, trending, montoring, alerting | post-failure-analysis, auditing, diagnostic |
| Traits | timestamp, searching,  | timestamp, searching, levelling