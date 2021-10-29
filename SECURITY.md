<!-- @bot-written -->
<!-- % protected region % [Configure security document here] off begin -->
# Security

The purpose of this document is to outline and provide guidance on the security considerations of your application. As the security landscape is always changing, this document does not claim to cover all aspects of your application. Note that the ultimate responsibility for making this application secure is on the developer.

This guide aims to provide guidance on known areas of risk and considerations.

For the purposes of this document, the [Application Security Verification Standard](https://github.com/OWASP/ASVS/tree/v4.0.2#latest-stable-version---402) will be used as the primary source of OWASP considerations.

## Codebots Platform

Your application can be configured with out-of-the-box security configurations using the [Security Diagram](https://codebots.com/docs/using-the-security-diagram). This will enable you to set CRUD permissions on each of your entities and describe which users have access to the pages defined in the UI model. You can also specify which users have access to the back-end.

## Application Summary

While these default settings are a great start, it is the responsibility of the developer when making protected region changes to consider any custom security methods or endpoints they add to remain OWASP compliant. It is important to understand AAA (Application, Authentication, Auditing) to keep your application secure. To gain a better understanding for how AAA security works in your C#Bot application, please refer to our documentation on [handling security with C#Bot applications](https://codebots.com/docs/handling-security-in-c-bot-server-side).

For an in-depth guide of performing custom security, please refer to our guide on [custom security with C#Bot](https://codebots.com/docs/c-bot-custom-security).

## Two-Factor Authentication

Your application comes with two-factor authentication out of the box. The default options available are email and authenticator app. You can find most of the configuration files in `./serverside/src/services/TwoFactor`.

Two-factor authentication is an optional feature, it is disabled by default for all users. It can be enabled and configured per user by an administrator in the all users page.

**NOTE: If Two-factor authentication is enabled, a user will be unable to use the JWT authentication scheme.**


## Development

### v4.0.2-14.4.4 Verify that all responses contain X-Content-Type-Options: nosniff

**NOTE: The default behaviour is inline with OWASP recommendations**.

X-Content-Type-Options is set to `nosniff`. This will prevent the browser from MIME-sniffing a response away from the declared content-type.

This can be overwritten in the `UseSecurityHeaders` method of the `serverside/src/Utility/ApplicationBuilderExtensions.cs` class.

See the [OWASP Secure headers project for details](https://wiki.owasp.org/index.php/OWASP_Secure_Headers_Project#xcto).

### v4.0.2-14.4.6 Verify that a suitable "Referrer-Policy" header is included

**NOTE: The default behaviour is inline with OWASP recommendations**.

Referrer policy is by default set to `no-referrer`. This means that the `Referer` header will be omitted entirely, and no referrer information is sent along with requests.

This  can be overwritten in the `UseSecurityHeaders` method of the `serverside/src/Utility/ApplicationBuilderExtensions.cs` class.

See the [OWASP Secure headers project for details](https://wiki.owasp.org/index.php/OWASP_Secure_Headers_Project#rp).

### v4.0.2-14.4.7 Verify that a suitable X-Frame-Options or Content-Security-Policy is in use

**NOTE: The default behaviour is inline with OWASP recommendations**.

X-Frame-Options is set to `sameorigin`. This will prevent rendering if the origin is a mismatch.

This can be overwritten in the `UseSecurityHeaders` method of the `serversider/src/Utility/ApplicationBuilderExtensions.cs`.

See the [OWASP Secure headers project for details](https://wiki.owasp.org/index.php/OWASP_Secure_Headers_Project#xfo).

### v4.0.2-2.2.1 Rate Limiting

To assist in the prevention of denial of service attacks, IP rate limiting has been built into the project. This is can be configured both globally and per API endpoint. The rate limited is determined by the IP address of the incoming connection. Rate limiting can be configured in `serverside/src/appsettings.xml`. An annotated rate limiting config is displayed below. For comprehensive documentation of all settings that can be configured, see [here](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware).

```xml
<IpRateLimiting>
	<!-- In the case that the server is behind an ingress server the IP address of the connection will be set to the IP address of the ingress instead of the client user. This field specifies the name of a header on the HTTP request that is set by the ingress that specifies the real IP address of the client. -->
	<RealIpHeader>X-Real-IP</RealIpHeader>
	<!-- The HTTP response code to respond with when the rate limit has been exceeded. -->
	<HttpStatusCode>429</HttpStatusCode>
	<!-- List of IP addresses that are excluded from rate limtiting. This accepts port ranges. -->
	<IpWhitelist name="0">127.0.0.1</IpWhitelist>
	<IpWhitelist name="1">192.168.0.0/16</IpWhitelist>
	<!-- Endpoints that are excluded from the rate limit rules. -->
	<EndpointWhitelist name="0">*:/api/health</EndpointWhitelist>
	<!-- Global rule for all endpoints that are not whitelisted. This will limit the number of requests by one IP address to 500 every 10 minutes. -->
	<GeneralRules name="0">
		<Endpoint>*</Endpoint>
		<Period>10m</Period>
		<Limit>500</Limit>
	</GeneralRules>
	<!-- Specific rule for a single endpoint. This will limit the number of POST requests to this single endpoint to 30 every minute. -->
	<GeneralRules name="1">
		<Endpoint>POST:/api/custom/endpoint</Endpoint>
		<Period>1m</Period>
		<Limit>30</Limit>
	</GeneralRules>
</IpRateLimiting>
```

## Deployment

While much of the security is handled by application, there are factors that must be considered when deploying it into an environment.

### 14.4.5 Verify that HTTP Strict Transport Security headers are included on all responses and for all subdomains

[HTTP Strict Transport Security](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Strict-Transport-Security) (HSTS) is a mechanism to allow web servers to enforce the use of TLS. While this can be implemented and controlled by the application, it is recommended to enable this in the outside boundary of the deployment infrastructure to decrease development complexity and increase operational maintainability.

For example, the configuration for a Nginx reverse proxy may look like:

```
server {
    server_name  .myserver.com
    location / {
        proxy_pass http://localhost:5000;
        add_header Strict-Transport-Securitymax-age=15724800; includeSubDomains;
    }
}
```

**NOTE: This will enforce HTTPS so make sure you also have your TLS certificates set and configured for your application otherwise you will lose access.**

<!-- % protected region % [Configure security document here] end -->
