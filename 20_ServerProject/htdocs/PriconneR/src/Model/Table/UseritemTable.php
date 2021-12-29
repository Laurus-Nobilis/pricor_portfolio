<?php
namespace App\Model\Table;

use Cake\ORM\Query;
use Cake\ORM\RulesChecker;
use Cake\ORM\Table;
use Cake\Validation\Validator;

/**
 * Useritem Model
 *
 * @property \App\Model\Table\UsersTable|\Cake\ORM\Association\BelongsTo $Users
 * @property \App\Model\Table\ItemsTable|\Cake\ORM\Association\BelongsTo $Items
 *
 * @method \App\Model\Entity\Useritem get($primaryKey, $options = [])
 * @method \App\Model\Entity\Useritem newEntity($data = null, array $options = [])
 * @method \App\Model\Entity\Useritem[] newEntities(array $data, array $options = [])
 * @method \App\Model\Entity\Useritem|bool save(\Cake\Datasource\EntityInterface $entity, $options = [])
 * @method \App\Model\Entity\Useritem|bool saveOrFail(\Cake\Datasource\EntityInterface $entity, $options = [])
 * @method \App\Model\Entity\Useritem patchEntity(\Cake\Datasource\EntityInterface $entity, array $data, array $options = [])
 * @method \App\Model\Entity\Useritem[] patchEntities($entities, array $data, array $options = [])
 * @method \App\Model\Entity\Useritem findOrCreate($search, callable $callback = null, $options = [])
 */
class UseritemTable extends Table
{

    /**
     * Initialize method
     *
     * @param array $config The configuration for the Table.
     * @return void
     */
    public function initialize(array $config)
    {
        parent::initialize($config);

        $this->setTable('useritem');
        $this->setDisplayField('id');
        $this->setPrimaryKey('id');

        // $this->belongsTo('Users', [
        //     'foreignKey' => 'user_id',
        //     'joinType' => 'INNER'
        // ]);
        // $this->belongsTo('Items', [
        //     'foreignKey' => 'item_id',
        //     'joinType' => 'INNER'
        // ]);
    }

    /**
     * Default validation rules.
     *
     * @param \Cake\Validation\Validator $validator Validator instance.
     * @return \Cake\Validation\Validator
     */
    public function validationDefault(Validator $validator)
    {
        $validator
            ->integer('id')
            ->allowEmpty('id', 'create');

        $validator
            ->integer('num')
            ->requirePresence('num', 'create')
            ->notEmpty('num');

        return $validator;
    }

    // /**
    //  * Returns a rules checker object that will be used for validating
    //  * application integrity.
    //  *
    //  * @param \Cake\ORM\RulesChecker $rules The rules object to be modified.
    //  * @return \Cake\ORM\RulesChecker
    //  */
    // public function buildRules(RulesChecker $rules)
    // {
    //     // $rules->add($rules->existsIn(['user_id'], 'Users'));
    //     // $rules->add($rules->existsIn(['item_id'], 'Items'));

    //     return $rules;
    // }

    /**
     * Returns the database connection name to use by default.
     *
     * @return string
     */
    public static function defaultConnectionName()
    {
        return 'user_db';
    }
}
