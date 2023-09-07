# New Relic Trace Test

## Synchronous

This example creates a root span with `n` number of child spans. Child spans are created 1 min apart. When the length of the root span exceeds 60 min it is not visible in New Relic

Here is a visual representation of the issue. Note that the `m` number of child spans are being created in the `...` section.

```mermaid
gantt
    title      New Relic Synchronous Trace Behavior
    dateFormat HH:mm
    axisFormat %H:%M

    section Working
    Root (missing) : 00:00, 55m
    C1             : 00:00, 1m
    C2             : 00:01, 1m
    "..."          : 00:02, 52m
    Cn             : 00:54, 1m

    section Not Working
    Root (missing) :crit, 00:00, 65m
    C1             :      00:00, 1m
    C2             :      00:01, 1m
    "..."          :      00:02, 62m
    Cn             :      01:04, 1m
```

## Asynchronous

This example creates 2 spans with a short duration. Between each span the program sleeps. When the duration between spans is greater than or equal to 10 minutes both spans are visible in NRDB but not in the tracing UI.

Here is a visual representation of the issue.

```mermaid
gantt
    title      New Relic Asynchronous Trace Behavior
    dateFormat mm:ss
    axisFormat %M:%S

    section Working
    Root                     :      00:00, 30s
    C1                       :      09:30, 30s

    section Not Working
    Root                     :      00:00, 30s
    C1 (missing in trace ui) :crit, 10:30, 30s
```
