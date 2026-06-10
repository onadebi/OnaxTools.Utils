>#### Version 2.2.5
This change is a minor update to accomodate for tenant Organizations in `OnaxTools.Dto.Identity.AppUserIdentity`.<br/>
This change facilitates the handling of tenant organizations within the `AppUserIdentity` class, allowing for better management and organization of user identities in a multi-tenant environment.<br/>
This update enhances the functionality of the `AppUserIdentity` class by enabling it to support tenant organizations, which can improve the overall user experience and provide more flexibility in managing user identities across different tenants.


<br/>

>#### Version 2.2.4

This change is a minor update to fix vulnerability bug in `MongoDB.Driver` version `3.3.0`.<br/> 
This updates the MongoDB driver from version `3.3.0` to version `3.9.0`, which includes a fix for the SharpCompress vulnerability `(CVE-2026-44788)`<br/>
'SharpCompress' `0.30.1` has a known moderate severity vulnerability, https://github.com/advisories/GHSA-6c8g-7p36-r338. 
<br/>This change updates the SharpCompres dependency to version `0.48.1`, which includes a fix for the known vulnerability.