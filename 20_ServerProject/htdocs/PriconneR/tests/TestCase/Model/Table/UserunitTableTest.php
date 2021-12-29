<?php
namespace App\Test\TestCase\Model\Table;

use App\Model\Table\UserunitTable;
use Cake\ORM\TableRegistry;
use Cake\TestSuite\TestCase;

/**
 * App\Model\Table\UserunitTable Test Case
 */
class UserunitTableTest extends TestCase
{

    /**
     * Test subject
     *
     * @var \App\Model\Table\UserunitTable
     */
    public $Userunit;

    /**
     * Fixtures
     *
     * @var array
     */
    public $fixtures = [
        'app.userunit',
        'app.users',
        'app.units'
    ];

    /**
     * setUp method
     *
     * @return void
     */
    public function setUp()
    {
        parent::setUp();
        $config = TableRegistry::getTableLocator()->exists('Userunit') ? [] : ['className' => UserunitTable::class];
        $this->Userunit = TableRegistry::getTableLocator()->get('Userunit', $config);
    }

    /**
     * tearDown method
     *
     * @return void
     */
    public function tearDown()
    {
        unset($this->Userunit);

        parent::tearDown();
    }

    /**
     * Test initialize method
     *
     * @return void
     */
    public function testInitialize()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }

    /**
     * Test validationDefault method
     *
     * @return void
     */
    public function testValidationDefault()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }

    /**
     * Test buildRules method
     *
     * @return void
     */
    public function testBuildRules()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }

    /**
     * Test defaultConnectionName method
     *
     * @return void
     */
    public function testDefaultConnectionName()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }
}
