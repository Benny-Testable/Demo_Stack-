<?php

declare(strict_types=1);

namespace Tests\Unit;

use App\Support\LegacyPricing;
use PHPUnit\Framework\TestCase;

final class LegacyPricingTest extends TestCase
{
    private LegacyPricing $pricing;

    protected function setUp(): void
    {
        $this->pricing = new LegacyPricing();
    }

    public function testUnknownPlanIsFree(): void
    {
        self::assertSame(0.0, $this->pricing->quote('mystery', 10, 'us', false));
    }

    public function testStarterBasePrice(): void
    {
        self::assertSame(9.0, $this->pricing->quote('starter', 5, 'us', false));
    }

    public function testStarterSeatOverageInEurope(): void
    {
        self::assertSame(38.4, $this->pricing->quote('starter', 14, 'eu', false));
    }

    public function testGrowthAnnualDiscountInUs(): void
    {
        self::assertSame(93.0, $this->pricing->quote('growth', 40, 'us', true));
    }

    public function testEnterpriseAnnualEuropeDiscount(): void
    {
        self::assertSame(148.5, $this->pricing->quote('enterprise', 20, 'eu', true));
    }

    public function testCouponApplies(): void
    {
        self::assertSame(8.1, $this->pricing->quote('starter', 3, 'us', false, 'STAR10'));
    }
}
