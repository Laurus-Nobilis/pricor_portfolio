<?php
namespace App\Test\TestCase\Model\Table;

use App\Model\Table\UseritemTable;
use Cake\ORM\TableRegistry;
use Cake\TestSuite\TestCase;

/**
 * App\Model\Table\UseritemTable Test Case
 */
class UseritemTableTest extends TestCase
{

    /**
     * Test subject
     *
     * @var \App\Model\Table\UseritemTable
     */
    public $Useritem;

    /**
     * Fixtures
     *
     * @var array
     */
    public $fixtures = [
        'app.useritem',
        'app.users',
        'app.items'
    ];

    /**
     * setUp method
     *
     * @return void
     */
    public function setUp()
    {
        parent::setUp();
        $config = TableRegistry::getTableLocator()->exists('Useritem') ? [] : ['className' => UseritemTable::class];
        $this->Useritem = TableRegistry::getTableLocator()->get('Useritem', $config);
    }

    /**
     * tearDown method
     *
     * @return void
     */
    public function tearDown()
    {
        unset($this->Useritem);

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
