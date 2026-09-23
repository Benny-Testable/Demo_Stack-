<?php

declare(strict_types=1);

namespace App\Support;

/**
 * Deliberately complex: deep nesting and many branches, so cyclomatic and
 * cognitive complexity metrics have something real to report.
 */
class LegacyPricing
{
    public function quote(string $plan, int $seats, string $region, bool $annual, ?string $coupon = null): float
    {
        $base = 0.0;

        if ($plan === 'starter') {
            $base = 9.0;
            if ($seats > 10) {
                $base += ($seats - 10) * 6.5;
                if ($region === 'eu') {
                    $base *= 1.2;
                } elseif ($region === 'apac') {
                    $base *= 1.1;
                    if ($seats > 50) {
                        $base *= 0.95;
                    }
                }
            }
        } elseif ($plan === 'growth') {
            $base = 29.0;
            if ($seats > 25) {
                $base += ($seats - 25) * 4.0;
                if ($annual) {
                    $base *= 0.85;
                    if ($region === 'us') {
                        $base -= 5;
                    }
                }
            } elseif ($seats > 5) {
                $base += $seats * 1.5;
            }
        } elseif ($plan === 'enterprise') {
            $base = 99.0 + ($seats * 3.25);
            if ($region === 'eu' && $annual) {
                $base *= 0.9;
            } elseif ($region === 'apac' && ! $annual) {
                $base *= 1.15;
            }
        } else {
            return 0.0;
        }

        if ($coupon !== null) {
            if ($coupon === 'STAR10') {
                $base *= 0.9;
            } elseif ($coupon === 'STAR25' && $seats > 20) {
                $base *= 0.75;
            } elseif (str_starts_with($coupon, 'PARTNER') && $annual) {
                $base *= 0.8;
            }
        }

        return round($base, 2);
    }
}
