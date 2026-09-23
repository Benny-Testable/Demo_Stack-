<?php

declare(strict_types=1);

namespace App\Http\Middleware;

use App\Services\TenantManager;
use Closure;
use Illuminate\Http\Request;
use Illuminate\Http\JsonResponse;
use RuntimeException;

class ResolveTenant
{
    public function __construct(private readonly TenantManager $tenants)
    {
    }

    public function handle(Request $request, Closure $next): mixed
    {
        $slug = $request->header('X-Tenant') ?? $this->slugFromHost($request->getHost());

        if ($slug === null || $slug === '') {
            return new JsonResponse(['error' => 'tenant_not_specified'], 400);
        }

        try {
            $this->tenants->switchTo($this->tenants->resolveBySlug($slug));
        } catch (RuntimeException $e) {
            return new JsonResponse(['error' => 'tenant_unavailable', 'detail' => $e->getMessage()], 404);
        }

        return $next($request);
    }

    private function slugFromHost(string $host): ?string
    {
        $parts = explode('.', $host);

        return count($parts) > 2 ? $parts[0] : null;
    }
}
